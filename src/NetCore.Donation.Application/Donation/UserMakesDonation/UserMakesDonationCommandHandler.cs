using MediatR;
using NetCore.Donation.Domain.Enums;
using NetCore.Donation.Domain.IRepositories;
using NetCore.Donation.Domain.SharedKernel;

namespace NetCore.Donation.Application.Donation.UserMakesDonation;

public class UserMakesDonationCommandHandler(
    IUnitOfWork unitOfWork,
    ICountryRepository countryRepository,
    IContactRepository contactRepository,
    IPaymentMethodRepository paymentMethodRepository,
    IPaymentScheduleRepository paymentScheduleRepository)
    : IRequestHandler<UserMakesDonationCommand, UserMakesDonationResult>
{
    public async Task<UserMakesDonationResult> Handle(
        UserMakesDonationCommand request,
        CancellationToken cancellationToken)
    {
        var country = await countryRepository.FindByIdAsync(request.CountryId);
        if (country is null)
        {
            throw new ArgumentException($"Country '{request.CountryId}' was not found.", nameof(request));
        }

        var contact = await contactRepository.FindByEmailAsync(request.Email, cancellationToken);
        if (contact is null)
        {
            contact = Domain.Entities.Contact.Create(
                request.FirstName,
                request.LastName,
                request.DateOfBirth,
                request.AddressLine,
                request.Email,
                request.PhoneNumber,
                request.CountryId,
                request.DoNotEmail,
                request.DoNotSms);
            await contactRepository.AddAsync(contact, cancellationToken);
        }
        else
        {
            contact.SetCommunicationPreferences(request.DoNotEmail, request.DoNotSms);
        }

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var interval = ResolveInterval(request);
        var paymentMethod = Domain.Entities.PaymentMethod.Create(contact.Id, request.PaymentMethodName);
        await paymentMethodRepository.AddAsync(paymentMethod, cancellationToken);

        var schedule = Domain.Entities.PaymentSchedule.Create(
            contact.Id,
            paymentMethod.Id,
            request.Amount,
            today,
            interval);
        schedule.RaiseDonationCreated(request.PaymentType);
        await paymentScheduleRepository.AddAsync(schedule, cancellationToken);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UserMakesDonationResult(
            contact.Id,
            paymentMethod.Id,
            schedule.Id,
            request.IsRecurring);
    }

    private static RecurringInterval ResolveInterval(UserMakesDonationCommand request)
    {
        if (!request.IsRecurring)
        {
            return RecurringInterval.OneOff;
        }

        if (request.RecurringInterval == RecurringInterval.OneOff)
        {
            throw new ArgumentException(
                "A recurring donation requires a recurring interval.",
                nameof(request));
        }

        return request.RecurringInterval;
    }
}

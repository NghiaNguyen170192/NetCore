using MediatR;
using Microsoft.EntityFrameworkCore;
using NetCore.Donation.Application.Contact.Create;
using NetCore.Donation.Application.PaymentMethod.Create;
using NetCore.Donation.Application.PaymentSchedule.Create;
using NetCore.Donation.Application.Receipt.Create;
using NetCore.Donation.Application.Transaction.Create;
using NetCore.Donation.Domain.Enums;
using NetCore.Donation.Domain.IRepositories;
using NetCore.Donation.Migration.Common.Interface;

namespace NetCore.Donation.Migration.Seeds.Base;

public sealed class DonationSeed(
    ISender dispatcher,
    ICountryRepository countryRepository,
    IContactRepository contactRepository) : IDataSeed
{
    public IEnumerable<Type> Dependencies => [typeof(CountrySeed)];

    public async Task SeedAsync()
    {
        if (await contactRepository.GetAll().AnyAsync())
        {
            return;
        }

        var country = await countryRepository.GetAll().OrderBy(item => item.Name).FirstAsync();
        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        var contactId = await dispatcher.Send(new CreateContactCommand(
            "Sample",
            "Donor",
            new DateOnly(1990, 1, 1),
            "1 Donation Street",
            "sample.donor@example.com",
            "+61 400 000 000",
            country.Id));

        var paymentMethodId = await dispatcher.Send(
            new CreatePaymentMethodCommand(contactId, "Demo bank account"));

        var scheduleId = await dispatcher.Send(new CreatePaymentScheduleCommand(
            contactId,
            paymentMethodId,
            50.00m,
            today,
            RecurringInterval.Monthly));

        var transactionId = await dispatcher.Send(new CreateTransactionCommand(
            50.00m,
            scheduleId,
            contactId,
            paymentMethodId,
            PaymentType.Bank,
            today,
            today));

        await dispatcher.Send(new CreateReceiptCommand(contactId, transactionId));
    }
}
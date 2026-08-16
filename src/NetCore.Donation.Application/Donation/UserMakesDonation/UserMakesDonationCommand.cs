using MediatR;
using NetCore.Donation.Domain.Enums;

namespace NetCore.Donation.Application.Donation.UserMakesDonation;

public sealed record UserMakesDonationCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string AddressLine,
    string Email,
    string PhoneNumber,
    Guid CountryId,
    decimal Amount,
    string PaymentMethodName,
    PaymentType PaymentType,
    bool IsRecurring,
    RecurringInterval RecurringInterval,
    bool DoNotEmail = false,
    bool DoNotSms = false) : IRequest<UserMakesDonationResult>;

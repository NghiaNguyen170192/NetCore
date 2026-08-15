using MediatR;

namespace NetCore.Donation.Application.Contact.Update;

public sealed record UpdateContactCommand(
    Guid Id,
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string AddressLine,
    string Email,
    string PhoneNumber,
    Guid CountryId,
    bool DoNotEmail,
    bool DoNotSms) : IRequest<bool>;

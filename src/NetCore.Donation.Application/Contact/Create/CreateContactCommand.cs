using MediatR;

namespace NetCore.Donation.Application.Contact.Create;

public sealed record CreateContactCommand(
    string FirstName,
    string LastName,
    DateOnly DateOfBirth,
    string AddressLine,
    string Email,
    string PhoneNumber,
    Guid CountryId) : IRequest<Guid>;
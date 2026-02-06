using NetCore.Domain.Messaging;

namespace NetCore.Application.Country.Delete;

public sealed record DeleteCountryCommand(Guid Id) : IRequest<bool>;
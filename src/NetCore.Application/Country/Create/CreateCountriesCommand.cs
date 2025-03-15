using MediatR;

namespace NetCore.Application.Country.Create;

public sealed record CreateCountryCommand(string Name, string CountryCode, string Alpha2, string Alpha3) : IRequest<Guid>;

public sealed record CreateCountriesCommand(IEnumerable<CreateCountryCommand> Countries) : IRequest<IEnumerable<Guid>>;

public static class CountryExtension
{
	public static Domain.Entities.Country ToDbEntity(this CreateCountryCommand request)
	{
		return new Domain.Entities.Country(
			request.Name,
			request.CountryCode,
			request.Alpha2,
			request.Alpha3);
	}
}
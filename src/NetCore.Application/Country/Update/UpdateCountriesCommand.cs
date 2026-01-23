using NetCore.Domain.Messaging;

namespace NetCore.Application.Country.Update;

public sealed record UpdateCountryCommand(Guid Id, string Name, string CountryCode, string Alpha2, string Alpha3) 
    : IRequest<bool>;

public sealed record UpdateCountriesCommand(IEnumerable<UpdateCountryCommand> Countries) 
    : IRequest<bool>;

public static class UpdateCountryExtension
{
	public static void UpdateEntity(this UpdateCountryCommand request, Domain.Entities.Country country)
	{
		country.UpdateDetails(request.Name, request.CountryCode, request.Alpha2, request.Alpha3);
	}
}

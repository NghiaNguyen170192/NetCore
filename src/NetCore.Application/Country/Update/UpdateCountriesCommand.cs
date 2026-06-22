using MediatR;

namespace NetCore.Application.Country.Update;

public sealed record UpdateCountriesCommand(IEnumerable<UpdateCountryCommand> Countries) : IRequest<bool>;

public static class UpdateCountryExtension
{
	public static void UpdateEntity(this UpdateCountryCommand request, Domain.Entities.Country country)
	{
		country.UpdateDetails(request.Name, request.CountryCode, request.Alpha2, request.Alpha3);
	}
}

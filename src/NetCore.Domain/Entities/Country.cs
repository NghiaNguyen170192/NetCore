#nullable disable

using NetCore.Domain.SharedKernel;

namespace NetCore.Domain.Entities;

public class Country : Entity, IAggregateRoot
{
	public string Name { get; private set; }
	public string CountryCode { get; private set; }
	public string Alpha2 { get; private set; }
	public string Alpha3 { get; private set; }

	private Country() { }

	public Country(string name, string countryCode, string alpha2, string alpha3)
	{
		Name = name;
		CountryCode = countryCode;
		Alpha2 = alpha2;
		Alpha3 = alpha3;
	}
}
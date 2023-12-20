using Microsoft.EntityFrameworkCore;
using NetCore.Domain.Entities;
using NetCore.Domain.IRepositories;

namespace NetCore.Infrastructure.Database.Repositories;
public class CountryRepository : ICountryRepository
{
	private readonly ApplicationDatabaseContext _context;

	public CountryRepository(ApplicationDatabaseContext applicationDatabaseContext)
	{
		_context = applicationDatabaseContext;
	}

	public async Task AddAsync(Country country, CancellationToken cancellationToken)
	{
		await _context.Countries.AddAsync(country, cancellationToken);
	}

	public async Task AddAsync(IEnumerable<Country> countries, CancellationToken cancellationToken)
	{
		await _context.Countries.AddRangeAsync(countries, cancellationToken);
	}

	public async Task<bool> IsExistAsync(string name, string countryCode, string alpha2, string alpha3)
	{
		var result = await _context.Countries
			.AsNoTracking()
			.AnyAsync(country =>
				country.Name == name && country.CountryCode == countryCode
					&& country.Alpha2 == alpha2 && country.Alpha3 == alpha3);

		return result;
	}

	public void Delete(Country country)
	{
		_context.Countries.Remove(country);
	}

	public void Delete(IEnumerable<Country> countries)
	{
		_context.Countries.RemoveRange(countries);
	}

	public async Task<Country> FindByIdAsync(Guid id)
	{
		return await _context.Countries.FindAsync(id);
	}

	public IQueryable<Country> GetAll()
	{
		return _context.Countries.AsNoTracking();
	}
}
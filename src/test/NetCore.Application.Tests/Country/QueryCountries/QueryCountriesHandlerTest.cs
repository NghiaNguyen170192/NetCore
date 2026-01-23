using NetCore.Domain.SharedKernel;

namespace NetCore.Application.Tests.Country.QueryCountries;

[TestClass]
public class QueryCountriesHandlerTest : BaseTest
{
	private readonly ICountryRepository _countryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public QueryCountriesHandlerTest()
	{
		var context = GetContext().Result;
		_unitOfWork = context;
		_countryRepository = new CountryRepository(context);
	}

	//[TestMethod]
	//public async Task QueryCountriesShouldReturnIQueryable()
	//{
	//	//Arrange
	//	var query = new Application.Country.QueryCountries.QueryCountries();
	//	var handler = new QueryCountriesHandler(_countryRepository);

	//	//Act 
	//	var result = await handler.Handle(query, default);

	//	//Assert	
	//	Assert.AreEqual(result.GetType(), typeof(IQueryable<QueryCountryDto>));
	//}
}
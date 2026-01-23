using NetCore.Application.Country.Create;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Tests.Country.Create;

[TestClass]
public class CreateCountryCommandHandlerTest: BaseTest
{
    private readonly ICountryRepository _countryRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly MockCacheRepository<Domain.Entities.Country> _cacheRepository;

	public CreateCountryCommandHandlerTest()
    {
	    var context = GetContext().Result;
	    _unitOfWork = context;
		_countryRepository = new CountryRepository(context);
		_cacheRepository = new MockCacheRepository<Domain.Entities.Country>();
    }

    [TestMethod]
    [DataRow]
    public async Task CreateCountriesCommand_ShouldReturnValidGuids()
    {
        //Arrange
        var command1 = new CreateCountryCommand("test 1", "999", "ab", "abc");
        var command2 = new CreateCountryCommand("test 2", "998", "xy", "xyz");
        var command3 = new CreateCountryCommand("test 3", "997", "jk", "jkl");

        var list = new List<CreateCountryCommand>
        {
            command1,
            command2,
            command3
        };

        var commands = new CreateCountriesCommand(list);

        var handler = new CreateCountriesCommandHandler(_unitOfWork, _countryRepository, _cacheRepository);

        //Act
        var ids = await handler.HandleAsync(commands, default);

        //Assert
        Assert.IsNotNull(ids);
        Assert.AreEqual(ids.Count(), commands.Countries.Count());

        foreach (var id in ids)
        {
			Assert.AreNotEqual(id, Guid.Empty);
		}

		Assert.AreEqual(1, _cacheRepository.AddAsyncBulkCallCount);
    }

	[TestMethod]
	[DataRow]
	public async Task CreateCountryCommand_ShouldReturnValidGuid()
	{
		//Arrange
		var command = new CreateCountryCommand("test country", "100", "tc", "tst");
		var handler = new CreateCountriesCommandHandler(_unitOfWork, _countryRepository, _cacheRepository);

		//Act
		var id = await handler.HandleAsync(command, default);

		//Assert
		Assert.AreNotEqual(id, Guid.Empty);
		
		var country = await _countryRepository.FindByIdAsync(id);
		Assert.IsNotNull(country);
		Assert.AreEqual("test country", country.Name);
		Assert.AreEqual("100", country.CountryCode);

		Assert.AreEqual(1, _cacheRepository.AddAsyncSingleCallCount);
	}
}

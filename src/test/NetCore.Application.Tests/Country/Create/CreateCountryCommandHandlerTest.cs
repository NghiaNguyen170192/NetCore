using NetCore.Application.Country.Create;
using NetCore.Domain.SharedKernel;

namespace NetCore.Application.Tests.Country.Create;

[TestClass]
public class CreateCountryCommandHandlerTest: BaseTest
{
    private readonly ICountryRepository _countryRepository;
	private readonly IUnitOfWork _unitOfWork;

	public CreateCountryCommandHandlerTest()
    {
	    var context = GetContext().Result;
	    _unitOfWork = context;
		_countryRepository = new CountryRepository(context);
    }

    [TestMethod]
    [DataRow]
    public async Task CommandHandlerShouldReturnValidGuid()
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

        var handler = new CreateCountriesCommandHandler(_unitOfWork, _countryRepository);

        //Act
        var ids= await handler.Handle(commands, default);

        //Assert
        Assert.IsNotNull(ids);
        Assert.AreEqual(ids.Count(), commands.Countries.Count());

        foreach (var id in ids)
        {
			Assert.AreNotEqual(id, Guid.Empty);
		}
    }
}

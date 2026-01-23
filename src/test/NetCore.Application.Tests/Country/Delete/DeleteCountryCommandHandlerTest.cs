using NetCore.Application.Country.Delete;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Application.Tests.Country.Delete;

[TestClass]
public class DeleteCountryCommandHandlerTest : BaseTest
{
    private readonly ICountryRepository _countryRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly MockCacheRepository<Domain.Entities.Country> _cacheRepository;

    public DeleteCountryCommandHandlerTest()
    {
        var context = GetContext().Result;
        _unitOfWork = context;
        _countryRepository = new CountryRepository(context);
        _cacheRepository = new MockCacheRepository<Domain.Entities.Country>();
    }

    [TestMethod]
    [DataRow]
    public async Task DeleteCountryCommand_ShouldDeleteCountryAndInvalidateCache()
    {
        // Arrange - Create a country first
        var country = new Domain.Entities.Country("Test Country", "001", "TC", "TST");
        await _countryRepository.AddAsync(country, default);
        await _unitOfWork.SaveChangesAsync(default);

        var deleteCommand = new DeleteCountryCommand(country.Id);
        var handler = new DeleteCountriesCommandHandler(_unitOfWork, _countryRepository, _cacheRepository);

        // Act
        var result = await handler.HandleAsync(deleteCommand, default);

        // Assert
        Assert.IsTrue(result);

        var deletedCountry = await _countryRepository.FindByIdAsync(country.Id);
        Assert.IsNull(deletedCountry);

        Assert.AreEqual(1, _cacheRepository.DeleteAsyncCallCount);
    }

    [TestMethod]
    [DataRow]
    public async Task DeleteCountryCommand_WithNonExistentId_ShouldReturnFalse()
    {
        // Arrange
        var nonExistentId = Guid.NewGuid();
        var deleteCommand = new DeleteCountryCommand(nonExistentId);
        var handler = new DeleteCountriesCommandHandler(_unitOfWork, _countryRepository, _cacheRepository);

        // Act
        var result = await handler.HandleAsync(deleteCommand, default);

        // Assert
        Assert.IsFalse(result);
        Assert.AreEqual(0, _cacheRepository.DeleteAsyncCallCount);
    }

    [TestMethod]
    [DataRow]
    public async Task DeleteCountriesCommand_ShouldDeleteMultipleCountriesAndInvalidateCache()
    {
        // Arrange - Create multiple countries
        var country1 = new Domain.Entities.Country("Country 1", "001", "C1", "COU1");
        var country2 = new Domain.Entities.Country("Country 2", "002", "C2", "COU2");
        var country3 = new Domain.Entities.Country("Country 3", "003", "C3", "COU3");

        await _countryRepository.AddAsync(new[] { country1, country2, country3 }, default);
        await _unitOfWork.SaveChangesAsync(default);

        var deleteIds = new List<Guid> { country1.Id, country2.Id, country3.Id };
        var command = new DeleteCountriesCommand(deleteIds);
        var handler = new DeleteCountriesCommandHandler(_unitOfWork, _countryRepository, _cacheRepository);

        // Act
        var result = await handler.HandleAsync(command, default);

        // Assert
        Assert.IsTrue(result);

        var deleted1 = await _countryRepository.FindByIdAsync(country1.Id);
        Assert.IsNull(deleted1);

        var deleted2 = await _countryRepository.FindByIdAsync(country2.Id);
        Assert.IsNull(deleted2);

        var deleted3 = await _countryRepository.FindByIdAsync(country3.Id);
        Assert.IsNull(deleted3);

        Assert.AreEqual(3, _cacheRepository.DeleteAsyncCallCount);
    }
}

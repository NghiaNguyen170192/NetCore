using NetCore.Domain.IRepositories;
using NetCore.Domain.Messaging;
using NetCore.Domain.SharedKernel;

namespace NetCore.Application.Country.Update;

public class UpdateCountriesCommandHandler(
    IUnitOfWork unitOfWork,
    ICountryRepository countryRepository,
    ICacheRepository<Domain.Entities.Country> cacheRepository)
    : IRequestHandler<UpdateCountriesCommand, bool>,
      IRequestHandler<UpdateCountryCommand, bool>
{
    public async Task<bool> HandleAsync(UpdateCountriesCommand request, CancellationToken cancellationToken = default)
    {
        foreach (var countryCommand in request.Countries)
        {
            var country = await countryRepository.FindByIdAsync(countryCommand.Id);
            if (country is not null)
            {
                countryCommand.UpdateEntity(country);
                await cacheRepository.UpdateAsync(country);
            }
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> HandleAsync(UpdateCountryCommand request, CancellationToken cancellationToken = default)
    {
        var country = await countryRepository.FindByIdAsync(request.Id);
        if (country is null)
        {
            return false;
        }

        request.UpdateEntity(country);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        // Update cache
        await cacheRepository.UpdateAsync(country);

        return true;
    }
}
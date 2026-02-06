using NetCore.Domain.IRepositories;
using NetCore.Domain.Messaging;
using NetCore.Domain.SharedKernel;

namespace NetCore.Application.Country.Delete;

public class DeleteCountriesCommandHandler(
    IUnitOfWork unitOfWork,
    ICountryRepository countryRepository,
    ICacheRepository<Domain.Entities.Country> cacheRepository)
    : IRequestHandler<DeleteCountriesCommand, bool>,
      IRequestHandler<DeleteCountryCommand, bool>
{
    public async Task<bool> HandleAsync(DeleteCountriesCommand request, CancellationToken cancellationToken = default)
    {
        var countries = new List<Domain.Entities.Country>();

        foreach (var id in request.Ids)
        {
            var country = await countryRepository.FindByIdAsync(id);
            if (country is not null)
            {
                countries.Add(country);
                await cacheRepository.DeleteAsync(country);
            }
        }

        countryRepository.Delete(countries);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> HandleAsync(DeleteCountryCommand request, CancellationToken cancellationToken = default)
    {
        var country = await countryRepository.FindByIdAsync(request.Id);
        if (country is null)
        {
            return false;
        }

        countryRepository.Delete(country);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheRepository.DeleteAsync(country);

        return true;
    }
}
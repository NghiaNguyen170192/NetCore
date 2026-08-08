using MediatR;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Application.Country.Create;

public class CreateCountriesCommandHandler(
    IUnitOfWork unitOfWork,
    ICountryRepository countryRepository,
    ICacheRepository<Domain.Entities.Country> cacheRepository)
    : MediatR.IRequestHandler<CreateCountriesCommand, IEnumerable<Guid>>,
      MediatR.IRequestHandler<CreateCountryCommand, Guid>
{
    // MediatR handler methods
    public async Task<IEnumerable<Guid>> Handle(CreateCountriesCommand request, CancellationToken cancellationToken)
    {
        var countries = request.Countries.Select(ToDbEntity).ToList();

        await countryRepository.AddAsync(countries, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheRepository.AddAsync(countries);

        return countries.Select(x => x.Id);
    }

    public async Task<Guid> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        var country = request.ToDbEntity();
        await countryRepository.AddAsync(country, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheRepository.AddAsync(country);

        return country.Id;
    }

    private Domain.Entities.Country ToDbEntity(CreateCountryCommand request)
    {
        return Domain.Entities.Country.Create(request.Name, request.CountryCode, request.Alpha2, request.Alpha3);
    }
}

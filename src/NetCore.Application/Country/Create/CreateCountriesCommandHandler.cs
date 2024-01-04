using MediatR;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;

namespace NetCore.Application.Country.Create;

public class CreateCountriesCommandHandler :
    IRequestHandler<CreateCountriesCommand, IEnumerable<Guid>>,
    IRequestHandler<CreateCountryCommand, Guid>
    
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly ICountryRepository _countryRepository;

	public CreateCountriesCommandHandler(
		IUnitOfWork unitOfWork,
		ICountryRepository countryRepository)
	{
		_unitOfWork = unitOfWork;
		_countryRepository = countryRepository;
	}

	public async Task<IEnumerable<Guid>> Handle(CreateCountriesCommand request, CancellationToken cancellationToken)
	{
		var countries = request.Countries.Select(ToDbEntity).ToList();

        await _countryRepository.AddAsync(countries, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return countries.Select(x => x.Id);
	}

    public async Task<Guid> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
		var country = request.ToDbEntity();
        await _countryRepository.AddAsync(country, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

		return country.Id;
    }

    private Domain.Entities.Country ToDbEntity(CreateCountryCommand request)
	{
		return new Domain.Entities.Country(
			request.Name,
			request.CountryCode,
			request.Alpha2,
			request.Alpha3);
	}
}
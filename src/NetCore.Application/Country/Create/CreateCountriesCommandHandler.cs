using MediatR;
using NetCore.Domain.IRepositories;
using NetCore.Domain.SharedKernel;

namespace NetCore.Application.Country.Create;

public class CreateCountriesCommandHandler : IRequestHandler<CreateCountriesCommand, IEnumerable<Guid>>
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

		await AddToDbAsync(countries, cancellationToken);

		return countries.Select(x => x.Id);
	}

	private async Task AddToDbAsync(IEnumerable<Domain.Entities.Country> countries, CancellationToken cancellationToken)
	{
		await _countryRepository.AddAsync(countries, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);
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
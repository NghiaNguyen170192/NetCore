using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonRequest, CreatePersonResponse>
    {
        private readonly IRepository<Person> _repository;

        public CreatePersonHandler(IRepository<Person> repository)
        {
            _repository = repository;
        }

        public async Task<CreatePersonResponse> Handle(CreatePersonRequest request, CancellationToken cancellationToken)
        {
            if (await Exist(request))
            {
                //todo: handle exception
            }

            var person = Map(request);
            await _repository.AddAsync(person, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(MapResponse(person));
        }

        private async Task<bool> Exist(CreatePersonRequest request)
        {
            return await _repository.ExistAsync(person => person.FirstName == request.FirstName && person.LastName == request.LastName && person.Email == request.Email);
        }

        private Person Map(CreatePersonRequest request)
        {
            return new Person
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Phone = request.Phone,
                BirthDate = request.BirthDate,
                Website =  request.Website,
            };
        }

        private CreatePersonResponse MapResponse(Person person)
        {
            return new CreatePersonResponse(person.Id);
        }
    }
}

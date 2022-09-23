using MediatR;
using NetCore.Infrastructure.Database.Entities;
using System.Threading;
using System.Threading.Tasks;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class DeletePersonHandler : IRequestHandler<DeletePersonRequest>
    {
        private readonly IRepository<Person> _repository;

        public DeletePersonHandler(IRepository<Person> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeletePersonRequest request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetByIdAsync(request.Id);
            _repository.Remove(person);

            return await Task.FromResult(Unit.Value);
        }
    }
}

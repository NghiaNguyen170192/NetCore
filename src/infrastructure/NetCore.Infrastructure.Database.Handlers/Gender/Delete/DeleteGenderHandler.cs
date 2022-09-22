using MediatR;
using NetCore.Infrastructure.Database.Entities;
using System.Threading;
using System.Threading.Tasks;
using NetCore.Infrastructure.Database.Repositories;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class DeleteGenderHandler : IRequestHandler<DeleteGenderRequest>
    {
        private readonly IRepository<Person> _repository;

        public DeleteGenderHandler(IRepository<Person> repository)
        {
            _repository = repository;
        }

        public async Task<Unit> Handle(DeleteGenderRequest request, CancellationToken cancellationToken)
        {
            var person = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _repository.Remove(person);

            return await Task.FromResult(Unit.Value);
        }
    }
}

using MediatR;
using NetCore.Infrastructure.Database.Contexts;
using NetCore.Infrastructure.Database.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Handlers
{
    public class DeletePersonHandler : IRequestHandler<DeletePersonRequest>
    {
        private readonly DatabaseContext _databaseContext;

        public DeletePersonHandler(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public async Task<Unit> Handle(DeletePersonRequest request, CancellationToken cancellationToken)
        {
            var person = _databaseContext.Set<Person>().Single(x => x.Id == request.Id);
            _databaseContext.Set<Person>().Remove(person);

            return await Task.FromResult(Unit.Value);
        }
    }
}

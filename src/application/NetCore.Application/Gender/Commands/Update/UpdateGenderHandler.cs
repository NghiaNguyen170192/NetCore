using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Application.Repositories;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Application.Commands;

public class UpdateGenderHandler : IRequestHandler<UpdateGenderCommand>
{
    private readonly IRepository<Gender> _repository;

    public UpdateGenderHandler(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateGenderCommand request, CancellationToken cancellationToken)
    {
        var existingEntity = await _repository.GetByIdAsync(request.Id);
        if (existingEntity == null)
        {
            //todo: handle exception
            throw new NotImplementedException();
        }

        existingEntity.Name = request.Name;

        _repository.Update(existingEntity);
        await _repository.SaveChangesAsync();

        return Unit.Value;
    }
}

using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Application.Commands;

public class CreateGenderHandler : IRequestHandler<CreateGenderCommand, Guid>
{
    private readonly IRepository<Gender> _repository;

    public CreateGenderHandler(IRepository<Gender> repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
    {
        if (await Exist(request))
        {
            //todo: handle exception
        }

        var entity = Map(request);
        await _repository.AddAsync(entity);
        await _repository.SaveChangesAsync();

        return entity.Id;
    }

    private async Task<bool> Exist(CreateGenderCommand request)
    {
        return await _repository.ExistAsync(entity => entity.Name == request.Name);
    }

    private Gender Map(CreateGenderCommand request)
    {
        return new Gender
        {
            Name = request.Name,
        };
    }
}

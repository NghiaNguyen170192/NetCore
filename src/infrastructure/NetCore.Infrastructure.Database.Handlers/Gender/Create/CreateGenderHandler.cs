using MediatR;
using NetCore.Infrastructure.Database.Entities;
using NetCore.Infrastructure.Database.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NetCore.Infrastructure.Database.Handlers
{
    public class CreateGenderHandler : IRequestHandler<CreateGenderCommand, CreateGenderResponse>
    {
        private readonly IRepository<Gender> _repository;

        public CreateGenderHandler(IRepository<Gender> repository)
        {
            _repository = repository;
        }

        public async Task<CreateGenderResponse> Handle(CreateGenderCommand request, CancellationToken cancellationToken)
        {
            if (await Exist(request))
            {
                //todo: handle exception
            }

            var gender = Map(request);
            await _repository.AddAsync(gender, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return await Task.FromResult(MapResponse(gender));
        }

        private async Task<bool> Exist(CreateGenderCommand request)
        {
            return await _repository.ExistAsync(gender => gender.Name == request.Name);
        }

        private Gender Map(CreateGenderCommand request)
        {
            return new Gender
            {
                Name = request.Name,
            };
        }

        private CreateGenderResponse MapResponse(Gender Gender)
        {
            return new CreateGenderResponse(Gender.Id);
        }
    }
}

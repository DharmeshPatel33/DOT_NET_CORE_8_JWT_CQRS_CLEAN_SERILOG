using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entity;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sample.Commannd
{
    public class AddSampleEntityCommand : ITransactionCommand<AddSampleEntityCommandComplete>
    {
        public Guid Id { get; set; }

        public DateTime EventTime { get; set; }

        public string? Description { get; set; }

    }

    public record AddSampleEntityCommandComplete
    {
        public bool IsSuccess { get; set; }
    }

    public class AddSampleEntityCommandHandler : IRequestHandler<AddSampleEntityCommand, AddSampleEntityCommandComplete>
    {
        private readonly ILogger<AddSampleEntityCommandHandler> _logger;
        private readonly IRepository<SampleEntity> _repository;
        private readonly IIdentityService _identityService;

        public AddSampleEntityCommandHandler(ILogger<AddSampleEntityCommandHandler> logger, IRepository<SampleEntity> repository, IIdentityService identityService)
        {
            _logger = logger;
            _repository = repository;
            _identityService = identityService;
        }

        public async Task<AddSampleEntityCommandComplete> Handle(AddSampleEntityCommand request, CancellationToken cancellationToken)
        {
            await _repository.Add(new SampleEntity()
            {
                Id = request.Id,
                Description = request.Description,
                EventTime = request.EventTime,
                UserId = _identityService.GetUserId(),
            });

            _logger.LogInformation("Command Executed Id:{Id};Description:{Description};EventTime:{EventTime}", request.Id, request.Description, request.EventTime);

            return new AddSampleEntityCommandComplete() { IsSuccess = true };
        }
    }
}

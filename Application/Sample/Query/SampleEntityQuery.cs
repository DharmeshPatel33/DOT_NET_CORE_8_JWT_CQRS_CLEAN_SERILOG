using Application.Interfaces.Repository;
using Domain.Entity;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Application.Sample.Query.SampleEntityQuery;

namespace Application.Sample.Query
{
    public class SampleEntityQuery
        : IRequest<IEnumerable<SampleEntityQueryComplete>>
    {
        public record SampleEntityQueryComplete
        {
            public Guid Id { get; set; }

            public DateTime EventTime { get; set; }

            public string? Description { get; set; }
        }
    }
    public class SampleQueryHandler : IRequestHandler<SampleEntityQuery, IEnumerable<SampleEntityQueryComplete>>
    {
        private readonly IRepository<SampleEntity> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public SampleQueryHandler(IRepository<SampleEntity> repository, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IEnumerable<SampleEntityQueryComplete>> Handle(SampleEntityQuery request, CancellationToken cancellationToken)
        {
            var user = _httpContextAccessor.HttpContext.User;

            //var userId= user.Claims.FirstOrDefault(x=>x.)

            return (await _repository.GetAll())
                   .Select(x => new SampleEntityQueryComplete()
                   {
                       Id = x.Id,
                       Description = x.Description,
                       EventTime = x.EventTime
                   });
        }

    }
}

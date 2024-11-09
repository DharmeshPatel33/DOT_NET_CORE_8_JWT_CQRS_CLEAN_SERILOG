using Application.Authentication.Query;
using Application.Sample.Commannd;
using Application.Sample.Query;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Application.Sample.Query.SampleEntityQuery;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="User")]
    public class SampleController : ApiControllerBase
    {

        public SampleController(IMediator mediator):base(mediator)
        {

        }

        [HttpGet]
        public async Task<IEnumerable<SampleEntityQueryComplete>> GetAll()
        {
           return await Mediator.Send(new SampleEntityQuery());
        }

        [HttpPost]
        public async Task<AddSampleEntityCommandComplete> AddSample([FromBody] AddSampleEntityCommand addSampleEntityCommand)
        {
            return await Mediator.Send(addSampleEntityCommand);
        }
    }
}

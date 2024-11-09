using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        private readonly ISender _mediator;
        public ApiControllerBase(IMediator mediator) 
        {
            _mediator = mediator;
        }

        protected ISender Mediator => _mediator ?? HttpContext.RequestServices.GetService<ISender>();
    }
}

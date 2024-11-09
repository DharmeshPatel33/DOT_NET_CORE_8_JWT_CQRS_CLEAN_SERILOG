using Application.Authentication.Command.Register;
using Application.Authentication.Query.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace APIService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ApiControllerBase
    {
        public AuthenticationController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginQuery model)
        {
            var response = await Mediator.Send(model);
            if (response == null) 
            {
                return Unauthorized();
            }
            return Ok(response);
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            var responnse = await Mediator.Send(registerCommand);
            if(responnse != null)
            {
                return Ok(responnse);
            }
            return BadRequest();
        }
    }
}

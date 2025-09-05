using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TextboxMailApp.Application.Features.Users.Commands;
using TextboxMailApp.Application.Features.Users.Queries;

namespace TextboxMailApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPost("Register")]
        public async Task<IActionResult> Register(CreateUserCommand createUserCommand)
        {
            return Ok(await _mediator.Send(createUserCommand));
        }

        [HttpGet("Login")]
        public async Task<IActionResult> Login([FromQuery] LoginQuery loginQuery)
        {
            return Ok(await _mediator.Send(loginQuery));
        }
    }
}

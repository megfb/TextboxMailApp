using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextboxMailApp.Application.Features.Users.Commands;

namespace TextboxMailApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpPut("Update")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand command)
        {
            return Ok(await _mediator.Send(command));
        }
    }
}

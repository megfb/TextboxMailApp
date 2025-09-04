using MediatR;
using Microsoft.AspNetCore.Mvc;
using TextboxMailApp.Application.Features.EmailMessages.Queries;
using TextboxMailApp.Application.Features.Users.Commands;
using TextboxMailApp.Application.Features.Users.Queries;

namespace TextboxMailApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("GetMails")]
        public async Task<IActionResult> Test(int pageNumber, int pageSize,string userId)
        {
            return Ok(await _mediator.Send(new GetLatestEmailsQuery(pageNumber, pageSize,userId)));
        }
        [HttpGet("RefreshMails")]
        public async Task<IActionResult> Refresh()
        {
            return Ok(await _mediator.Send(new RefreshEmailQuery()));
        }
        [HttpPost("Register")]
        public async Task<IActionResult> CreateUser(CreateUserCommand createUserCommand)
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

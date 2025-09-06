using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("GetMails")]
        [Authorize]
        public async Task<IActionResult> Test(int pageNumber, int pageSize)
        {
            return Ok(await _mediator.Send(new GetLatestEmailsQuery(pageNumber, pageSize)));
        }
        [HttpGet("RefreshMails")]
        [Authorize]
        public async Task<IActionResult> Refresh()
        {
            return Ok(await _mediator.Send(new RefreshEmailQuery()));
        }
    }
}

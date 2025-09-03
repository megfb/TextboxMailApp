using MediatR;
using Microsoft.AspNetCore.Mvc;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        [HttpGet("GetMails")]
        public async Task<IActionResult> Test(int pageNumber,int pageSize)
        {
            return Ok(await _mediator.Send(new GetLatestEmailsQuery(pageNumber,pageSize)));
        }
        [HttpGet("RefreshMails")]
        public async Task<IActionResult> Refresh()
        {
            return Ok(await _mediator.Send(new RefreshEmailQuery()));
        }
    }
}

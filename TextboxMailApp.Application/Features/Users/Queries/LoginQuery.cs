using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.Users.Queries
{
    public class LoginQuery : IRequest<ApiResult<string>>
    {
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}

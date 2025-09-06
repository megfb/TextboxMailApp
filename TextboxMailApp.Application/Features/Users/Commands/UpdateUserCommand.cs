using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.Users.Commands
{
    public class UpdateUserCommand : IRequest<ApiResult<UsersDto>>
    {
        public string UserName { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string EmailAddress { get; set; } = default!;
        public string EmailPassword { get; set; } = default!;
        public string ServerName { get; set; } = default!;
        public int Port { get; set; } = default!;
    }
}

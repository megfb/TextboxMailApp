using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.EmailMessages.Queries
{
    public class RefreshEmailQuery : IRequest<ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        public string UserId { get; set; }
    }
}

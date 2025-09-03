using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.EmailMessages.Queries
{
    public class GetLatestEmailsQuery : IRequest<ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetLatestEmailsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
        }

    }
}

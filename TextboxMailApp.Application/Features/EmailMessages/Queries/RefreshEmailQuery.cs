using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TextboxMailApp.Application.Common.Responses;

namespace TextboxMailApp.Application.Features.EmailMessages.Queries
{
    public class RefreshEmailQuery:IRequest<ApiResult<IEnumerable<EmailMessagesDto>>>
    {
    }
}

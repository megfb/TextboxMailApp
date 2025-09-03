using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
    public class RefreshEmailQueryHandler(IEmailReader emailReader, IEmailMessageRepository emailMessageRepository, IUnitOfWork unitOfWork) :
        IRequestHandler<RefreshEmailQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        private readonly IEmailMessageRepository _emailMessageRepository = emailMessageRepository ?? throw new ArgumentNullException(nameof(emailMessageRepository));
        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(RefreshEmailQuery request, CancellationToken cancellationToken)
        {
            var lastEmail = await _emailMessageRepository.GetLatestAsync();
            uint lastUid = lastEmail?.Uid ?? 0;
            var newEmails = await _emailReader.GetEmailsAfterUidAsync(lastUid);
            if (!newEmails.Any())
                return ApiResult<IEnumerable<EmailMessagesDto>>.Success(new List<EmailMessagesDto>());

            await _emailMessageRepository.SaveRangeAsync(newEmails.Select(nm => new Domain.Entities.EmailMessage
            {
                Id = Guid.NewGuid().ToString(),
                Uid = nm.Uid,
                Subject = nm.Subject,
                FromAddress = nm.FromAddress,
                FromName = nm.FromName,
                To = nm.To,
                Date = nm.Date.ToUniversalTime(),
                Body = nm.Body,
                Snippet = nm.Snippet,
                Cc = nm.Cc,
                CreatedAt = nm.CreatedAt,
                UpdatedAt = nm.UpdatedAt,
            }).ToList());

            await _unitOfWork.SaveChangesAsync();

            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(newEmails);

        }
    }
}

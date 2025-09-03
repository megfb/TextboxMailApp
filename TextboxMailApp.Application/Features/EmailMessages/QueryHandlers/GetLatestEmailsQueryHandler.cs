using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
    public class GetLatestEmailsQueryHandler(IEmailReader emailReader, IEmailMessageRepository emailMessageRepository,IUnitOfWork unitOfWork) : IRequestHandler<GetLatestEmailsQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        private readonly IEmailMessageRepository _emailMessageRepository = emailMessageRepository ?? throw new ArgumentNullException(nameof(emailMessageRepository));
        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(GetLatestEmailsQuery request, CancellationToken cancellationToken)
        {

            // önce DB’den var mı kontrol et
            // Automapper kullanılabilir. Burada manuel yapıldı.
            var existingEmails = await _emailMessageRepository.GetAllByPageAsync(request.PageNumber, request.PageSize);
            if (existingEmails.Any())
            {
                var mailsDto = existingEmails.Select(dto => new EmailMessagesDto
                {
                    Id = dto.Id,
                    Uid = dto.Uid,
                    Subject = dto.Subject,
                    FromAddress = dto.FromAddress,
                    FromName = dto.FromName,
                    To = dto.To,
                    Date = dto.Date,
                    Body = dto.Body,
                    Snippet = dto.Snippet,
                    Cc = dto.Cc,
                    CreatedAt = dto.CreatedAt,
                    UpdatedAt = dto.UpdatedAt
                }).ToList();

                return ApiResult<IEnumerable<EmailMessagesDto>>.Success(mailsDto);
            }

            // DB’de yoksa Mail’den çek
            var emails = await _emailReader.GetEmailsByPageAsync(request.PageNumber, request.PageSize);

            var mails = emails.Select(nm => new EmailMessage
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
            }).ToList();
            // DB’ye kaydet
            await _emailMessageRepository.SaveRangeAsync(mails);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(emails);

        }
    }
}

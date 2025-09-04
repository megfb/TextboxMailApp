using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
    public class RefreshEmailQueryHandler(IEmailReader emailReader, IEmailMessageRepository emailMessageRepository, IUnitOfWork unitOfWork,IUserRepository userRepository) :
        IRequestHandler<RefreshEmailQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        private readonly IEmailMessageRepository _emailMessageRepository = emailMessageRepository ?? throw new ArgumentNullException(nameof(emailMessageRepository));
        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(RefreshEmailQuery request, CancellationToken cancellationToken)
        {
            request.UserId = "24465c7d-3acd-4b07-9d1c-aa619dd500c7";
            var user = await _userRepository.GetByIdAsync(request.UserId);
            var lastEmail = await _emailMessageRepository.GetLatestAsync(user.Id);
            uint lastUid = lastEmail?.Uid ?? 0;
            var newEmails = await _emailReader.GetEmailsAfterUidAsync(lastUid,user);
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
                UserId = nm.UserId
            }).ToList());

            await _unitOfWork.SaveChangesAsync();

            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(newEmails);

        }
    }
}

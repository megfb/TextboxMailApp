using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
    public class RefreshEmailQueryHandler(IEmailReader emailReader, IEmailMessageRepository emailMessageRepository, IUnitOfWork unitOfWork
        , IUserRepository userRepository, ICurrentUserService currentUserService, IMapper mapper) :
        IRequestHandler<RefreshEmailQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        private readonly IEmailMessageRepository _emailMessageRepository = emailMessageRepository ?? throw new ArgumentNullException(nameof(emailMessageRepository));
        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(RefreshEmailQuery request, CancellationToken cancellationToken)
        {
            //token içerisinden user id alındı
            string id = _currentUserService.UserId!;
            //db den user çekildi
            var user = await _userRepository.GetByIdAsync(id);
            //db de ki son mail alındı
            var lastEmail = await _emailMessageRepository.GetLatestAsync(user.Id);
            uint lastUid = lastEmail?.Uid ?? 0;
            //yeni mail varsa kıyaslanması için user ve uid gönderildi. user göndermemin sebebi mail bilgilerine erişmek için.
            var newEmails = await _emailReader.GetEmailsAfterUidAsync(lastUid, user);
            //yeni mail yoksa pas geç
            if (!newEmails.Any())
                return ApiResult<IEnumerable<EmailMessagesDto>>.Success(new List<EmailMessagesDto>());

            var mappedEmails = _mapper.Map<List<EmailMessage>>(newEmails);

            await _emailMessageRepository.SaveRangeAsync(mappedEmails);

            await _unitOfWork.SaveChangesAsync();

            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(newEmails);

        }
    }
}

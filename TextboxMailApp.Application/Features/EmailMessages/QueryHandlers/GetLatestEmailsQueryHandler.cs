using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
    public class GetLatestEmailsQueryHandler(IEmailReader emailReader, IEmailMessageRepository emailMessageRepository, IUserRepository userRepository, IUnitOfWork unitOfWork,
        IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetLatestEmailsQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        private readonly IEmailMessageRepository _emailMessageRepository = emailMessageRepository ?? throw new ArgumentNullException(nameof(emailMessageRepository));
        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(GetLatestEmailsQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId!;

            // 1. Eğer DB’de mail varsa, doğrudan DB’den dön
            var existingEmails = await _emailMessageRepository.GetAllByPageAsync(request.PageNumber, request.PageSize, userId);
            if (existingEmails.Any())
            {
                var mailsDto = _mapper.Map<IEnumerable<EmailMessagesDto>>(existingEmails);
                return ApiResult<IEnumerable<EmailMessagesDto>>.Success(mailsDto);
            }

            // 2. User bilgisi al
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ApiResult<IEnumerable<EmailMessagesDto>>.Fail("Kullanıcı bulunamadı");

            // 3. DB’deki en küçük UID’yi al
            var minExistingUid = await _emailMessageRepository.GetMinUidAsync(userId);
            // 4. Eğer DB boşsa → son 100 maili çek
            // Eğer minExistingUid varsa → ondan önceki 100’lük bloğu çek
            var emails = await _emailReader.GetEmailsByPageAsync(user, minExistingUid);
            var mails = _mapper.Map<IEnumerable<EmailMessage>>(emails);

            // 5. DB’ye kaydet
            await _emailMessageRepository.SaveRangeAsync(mails);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            //6. DB'den çek
            var Emails = await _emailMessageRepository.GetAllByPageAsync(request.PageNumber, request.PageSize, userId);
            var mailList = _mapper.Map<IEnumerable<EmailMessagesDto>>(Emails);
            //İlgili datayı dön
            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(mailList);
        }

    }
}

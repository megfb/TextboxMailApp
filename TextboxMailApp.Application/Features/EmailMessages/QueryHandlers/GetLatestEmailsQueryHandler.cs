using System.Security.Claims;
using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.EmailMessages.Queries;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.EmailMessages.QueryHandlers
{
    public class GetLatestEmailsQueryHandler(IEmailReader emailReader, IUserRepository userRepository,
        IMapper mapper, ICurrentUserService currentUserService) : IRequestHandler<GetLatestEmailsQuery, ApiResult<IEnumerable<EmailMessagesDto>>>
    {
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IEmailReader _emailReader = emailReader ?? throw new ArgumentNullException(nameof(emailReader));
        public async Task<ApiResult<IEnumerable<EmailMessagesDto>>> Handle(GetLatestEmailsQuery request, CancellationToken cancellationToken)
        {
            //token içerisinden user id alındı
            var id = _currentUserService.UserId!;

            var user = await _userRepository.GetByIdAsync(id!);
            if (user == null)
                return ApiResult<IEnumerable<EmailMessagesDto>>.Fail("Kullanıcı bulunamadı");

            //db de olmadığı için mail adresine bağlanıldı ve mailler çekildi
            var emails = await _emailReader.GetEmailsByPageAsync(request.PageNumber, request.PageSize, user);
            var mails = _mapper.Map<IEnumerable<EmailMessage>>(emails);

            return ApiResult<IEnumerable<EmailMessagesDto>>.Success(emails);

        }
    }
}

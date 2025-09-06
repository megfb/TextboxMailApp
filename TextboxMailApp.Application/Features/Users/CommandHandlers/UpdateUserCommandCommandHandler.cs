using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Api;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.Users.Commands;

namespace TextboxMailApp.Application.Features.Users.CommandHandlers
{
    public class UpdateUserCommandCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher, ICurrentUserService currentUserService) : IRequestHandler<UpdateUserCommand, ApiResult<UsersDto>>
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly ICurrentUserService _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
        public async Task<ApiResult<UsersDto>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null)
                return ApiResult<UsersDto>.Fail("User not found.", System.Net.HttpStatusCode.NotFound);

            user.UserName = request.UserName;
            user.EmailAddress = request.EmailAddress;
            user.EmailPassword = request.EmailPassword;
            user.ServerName = request.ServerName;
            user.Port = request.Port;
            user.UpdatedAt = DateTime.UtcNow;
            _userRepository.Update(user);

            await _unitOfWork.SaveChangesAsync();

            return ApiResult<UsersDto>.Success(_mapper.Map<UsersDto>(user));
        }
    }
}

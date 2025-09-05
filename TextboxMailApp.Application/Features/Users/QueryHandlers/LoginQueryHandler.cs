using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.Users.Queries;

namespace TextboxMailApp.Application.Features.Users.QueryHandlers
{
    public class LoginQueryHandler(IUserRepository userRepository, ITokenService tokenService, IMapper mapper, IPasswordHasher passwordHasher) : IRequestHandler<LoginQuery, ApiResult<string>>
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        private readonly ITokenService _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        public async Task<ApiResult<string>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByUserNameOrEmailAsync(request.UserName, request.UserName);

            if (user == null || !_passwordHasher.Verify(request.Password, user.PasswordHash))
                return ApiResult<string>.Fail("Invalid username or password");

            var token = _tokenService.CreateToken(user);

            return ApiResult<string>.Success(token);

        }
    }
}

using AutoMapper;
using MediatR;
using TextboxMailApp.Application.Common.Responses;
using TextboxMailApp.Application.Contracts.Persistence;
using TextboxMailApp.Application.Features.Users.Commands;
using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Features.Users.CommandHandlers
{
    public class CreateUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher passwordHasher) : IRequestHandler<CreateUserCommand, ApiResult<UsersDto>>
    {
        private readonly IPasswordHasher _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        private readonly IMapper _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        private readonly IUserRepository _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        public async Task<ApiResult<UsersDto>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userRepository.GetByUserNameOrEmailAsync(request.UserName, request.EmailAddress);
            if (existingUser != null)
                return ApiResult<UsersDto>.Fail("Username or Email already exists.");

            var user = new User
            {
                UserName = request.UserName,
                PasswordHash = _passwordHasher.Hash(request.PasswordHash),
                EmailAddress = request.EmailAddress,
                EmailPassword = request.EmailPassword,
                ServerName = request.ServerName,
                Port = request.Port,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return ApiResult<UsersDto>.Success(_mapper.Map<UsersDto>(user));
        }
    }
}

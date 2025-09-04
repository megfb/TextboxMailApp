using TextboxMailApp.Domain.Entities;

namespace TextboxMailApp.Application.Contracts.Persistence
{
    public interface ITokenService
    {
        public string CreateToken(User user);
    }
}

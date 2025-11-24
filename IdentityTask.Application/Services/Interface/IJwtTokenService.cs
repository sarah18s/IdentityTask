using IdentityTask.Domain;

namespace IdentityTask.Application.Services.Interface
{
    public interface IJwtTokenService
    {
        public string CreateToken(User user, IEnumerable<string> roles);
    }
}

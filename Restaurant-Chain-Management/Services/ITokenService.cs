using Restaurant_Chain_Management.Models;

namespace Restaurant_Chain_Management.Services
{
    public interface ITokenService
    {
        string CreateToken(ApplicationUser user, IList<string> roles);
    }
}

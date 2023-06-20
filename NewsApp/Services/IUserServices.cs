using Microsoft.AspNetCore.Identity;

namespace NewsApp.Services
{
    public interface IUserServices
    {
        Task<IdentityUser> GetUser();
    }
}

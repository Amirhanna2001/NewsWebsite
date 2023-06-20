using Microsoft.AspNetCore.Identity;

namespace NewsApp.Services
{
    public class UserServices :IUserServices
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserServices(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
         }

        public async Task<IdentityUser> GetUser()
        {
            return await _userManager.FindByEmailAsync("superAdmin@news.com");
        }
    }
}

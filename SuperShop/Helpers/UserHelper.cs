using Microsoft.AspNetCore.Identity;
using SuperShop.Data.Entities;
using System.Threading.Tasks;

namespace SuperShop.Helpers {
    public class UserHelper : IUserHelper {
        private readonly UserManager<User> userManager;

        public UserHelper(UserManager<User> userManager) {
            this.userManager = userManager;
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password) {
            return await userManager.CreateAsync(user, password);
        }

        public async Task<User> GetUserByEmailAsync(string email) {
            return await userManager.FindByEmailAsync(email);
        }
    }
}

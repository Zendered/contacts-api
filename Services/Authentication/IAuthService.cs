using ContactsApi.Models;

namespace ContactsApi.Services.Authentication
{
    public interface IAuthService
    {
        public Task<(bool res, string msg)> LoginAsync(string username, string password);

        public Task<(bool res, string msg)> RegisterAsync(User user, string password);

        public Task<bool> UserExistsAsync(string username);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.BLL.Services
{
    public interface IAuthService
    {
        // registration och login
        Task<UserDto?> RegisterAsync(string username, string password);
        Task<UserDto?> LoginAsync(string username, string password);
        Task LogoutAsync();

        // user management(?)
        Task<UserDto?> GetCurrentUserAsync();
        Task<UserDto?> GetUserByIdAsync(string userId);
        Task<bool> ChangeUsernameAsync(string userId, string newUsername);
        Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task<bool> DeleteAccountAsync(string userId, string password);

        // validation om vi ska ha det
        Task<bool> UsernameExistsAsync(string username);
        Task<bool> ValidatePasswordAsync(string userId, string password);
    }
}

using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;

namespace ChalkboardChat.BLL.Services
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync (string username, string password);
        Task<User?> LoginAsync (string username, string password);
        Task<bool> ChangeUsernameAsync(int userId, string newUsername);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        Task<bool> DeleteAccountAsync(int userId, string password);


    }
}


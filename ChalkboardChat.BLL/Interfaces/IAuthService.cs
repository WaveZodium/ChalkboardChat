using MessageBoardApp.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.BLL.Interfaces
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
        Task<bool> UpdateUsernameAsync(string userId, UpdateUsernameDto dto);
        Task<bool> UpdatePasswordAsync(string userId, UpdatePasswordDto dto);
        Task<bool> DeleteAccountAsync(string userId, DeleteAccountDto dto);

        // validation om vi ska ha det
        Task<bool> UsernameExistsAsync(string username);
       
    }
}


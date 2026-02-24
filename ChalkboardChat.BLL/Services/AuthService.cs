using ChalkboardChat.BLL.Interfaces;
using ChalkboardChat.DAL.Models;
using MessageBoardApp.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChalkboardChat.BLL.Services
{
    internal class AuthService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        // adapter methods to satisfy IAuthService interface
        // these map simple (username, password) calls to the existing DTO-based implementations

        public Task<UserDto?> RegisterAsync(string username, string password)
        {
            // create dto and delegate to existing method
            var dto = new RegisterDto
            {
                Username = username,
                Password = password
            };
            return RegisterAsync(dto);
        }

        public Task<UserDto?> LoginAsync(string username, string password)
        {
            // create dto and delegate to existing method
            var dto = new LoginDto
            {
                Username = username,
                Password = password
            };
            return LoginAsync(dto);
        }

        //registrering och login
        public async Task<UserDto?> RegisterAsync(RegisterDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Username))
                return null;

            if (await UsernameExistsAsync(dto.Username))
                return null;

            // skkapa användare via Identity
            var user = new ApplicationUser
            {
                UserName = dto.Username,
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return null;

            // logga in automatiskt (?
            await _signInManager.SignInAsync(user, isPersistent: false);

            // returnera UserDto
            return MapToUserDto(user);
        }

        public async Task<UserDto?> LoginAsync(LoginDto dto)
        {
            //validering
            if (string.IsNullOrWhiteSpace(dto.Username) || string.IsNullOrWhiteSpace(dto.Password))
                return null;

            //logga in via Identity
            var result = await _signInManager.PasswordSignInAsync(
                dto.Username,
                dto.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
                return null;

            // hämta användare
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null || user.IsDeleted)
                return null;

            return MapToUserDto(user);
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        //user manegement

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null)
                return null;

            var user = await _userManager.GetUserAsync(httpContext.User);
            if (user == null || user.IsDeleted)
                return null;

            return MapToUserDto(user);
        }

        public async Task<UserDto?> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return null;

            return MapToUserDto(user);
        }

        public async Task<bool> UpdateUsernameAsync(string userId, UpdateUsernameDto dto)
        {
            // vaalidera
            if (string.IsNullOrWhiteSpace(dto.NewUsername))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            // kolla om username finns
            if (await UsernameExistsAsync(dto.NewUsername) && user.UserName != dto.NewUsername)
                return false;

            //uppdatera via Identity
            var result = await _userManager.SetUserNameAsync(user, dto.NewUsername);
            if (!result.Succeeded)
                return false;

            // uppdatera authentication cookie (?
            await _signInManager.RefreshSignInAsync(user);

            return true;
        }

        public async Task<bool> UpdatePasswordAsync(string userId, UpdatePasswordDto dto)
        {
            // validera (igen)
            if (string.IsNullOrWhiteSpace(dto.NewPassword))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            // byt lösenord via Identity
            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (!result.Succeeded)
                return false;

            // uppdatera authentication cookie (?
            await _signInManager.RefreshSignInAsync(user);

            return true;
        }

        public async Task<bool> DeleteAccountAsync(string userId, DeleteAccountDto dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            // validate user exists and is active
            if (user == null || user.IsDeleted)
                return false;

            // verifiera lösenord
            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: false);
            if (!passwordCheck.Succeeded)
                return false;

            // business logic: soft delete
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return false;

            // logga ut
            await _signInManager.SignOutAsync();

            return true;
        }

        //validation
        public async Task<bool> UsernameExistsAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user != null && !user.IsDeleted;
        }

        private static UserDto MapToUserDto(ApplicationUser user)
        {
            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName ?? "Unknown",
                CreatedAt = user.CreatedAt,
                IsDeleted = user.IsDeleted
            };
        }
    }
}

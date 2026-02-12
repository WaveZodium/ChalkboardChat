using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ChalkboardChat.BLL.Services
{
    internal class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        //reigstrera ny användare

        public async Task<UserDto?> RegisterAsync(string username, string password)
        {
            // validera input
            if (string.IsNullOrWhiteSpace(username) || username.Length < 3 || username.Length > 50)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(password) || password.Length < 6)
            {
                return null;
            }

            // kolla om användarnamn finns
            if (await UsernameExistsAsync(username))
            {
                return null;
            }
        

        //ny identityuser
        var user = new IdentityUser
        {
            UserName = username,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return null;
        }

        // logga in automatiskt efter registrering (tack ai)
        //await _signInManager.SignInAsync(user, isPersistent: false);

        // reutrnar dto
        return MapToUserDto(user);
    }
}

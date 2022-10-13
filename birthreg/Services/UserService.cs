using birthreg.Data;
using birthreg.Models;
using birthreg.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace birthreg.Services
{
    public interface IUserService
    {
        Task<Tuple<bool, string>> ChangePasswordAsync(ChangePasswordViewModel model);
        Task<Tuple<string, User>> Login(string email, string password);
        Task<User> GetUser();
    }

    public class UserService : IUserService
    {
        private readonly BirthContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserService(BirthContext context, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<Tuple<bool, string>> ChangePasswordAsync(ChangePasswordViewModel model)
        {
            var user = await GetUser();
            if (user == null)
                return new Tuple<bool, string>(false, "There is no account with this email");
            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!isPasswordCorrect)
                return new Tuple<bool, string>(false, "Incorrect Password");

            else if (model.NewPassword != model.ConfirmNewPassword)
                return new Tuple<bool, string>(false, "New Passwords do not match");

            var result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);

            if (result.Succeeded)
            {
                return new Tuple<bool, string>(true, "Password Changed Succesfully");
            }
            else
                return new Tuple<bool, string>(false, result.Errors.Select(e => e.Description).ToString());
        }

        public async Task<Tuple<string, User>> Login(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new Tuple<string, User>("No user with such email", null);
            var result = await _userManager.CheckPasswordAsync(user, password);
            if (!result)
            {
                return new Tuple<string, User>("Incorrect Password", null);
            }
            var token = GenerateToken(user);
            return new Tuple<string, User>(token, user); ;
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {" "} {user.LastName}"),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUser()
        {
            var userID = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userID != null)
                return await _userManager.FindByIdAsync(userID);
            return null;
        }
    }
}

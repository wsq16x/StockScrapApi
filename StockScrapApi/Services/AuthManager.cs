using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StockScrapApi.Dtos;
using StockScrapApi.Models;
using System.CodeDom.Compiler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace StockScrapApi.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthManager> _logger;
        private User _user;

        public AuthManager(UserManager<User> userManager, IConfiguration configuration, ILogger<AuthManager> logger, User user)
        {
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return string.Empty;
        }

        private SigningCredentials GetSigningCredentials()
        {
            throw new NotImplementedException();
        }

        private async Task<List<Claim>> GetClaims()
        {
            return new List<Claim>();
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> ValidateUser(LoginDto userDTO)
        {
            _user = await _userManager.FindByNameAsync(userDTO.UserName);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDTO.Password));
        }
    }
}

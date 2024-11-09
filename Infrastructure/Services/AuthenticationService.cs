using Application.Interfaces;
using Application.Interfaces.Repository;
using Domain.Entity;
using Domain.Model;
using Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserRepository _userRepository;
        private readonly AppConfigurations _appConfigurations;
        public AuthenticationService(IUserRepository userRepository, AppConfigurations appConfigurations) 
        {
            _userRepository = userRepository;
            _appConfigurations = appConfigurations;
        }

        public async Task<LoginResponse> Login(string username, string password)
        {
            var user = await _userRepository.GetByUserNameAsync(username);
            if (user != null)
            {
                if(await _userRepository.CheckPasswordAsync(user, password))
                {
                    var token = GetJwtSecurityToken(user);


                    return new LoginResponse { Token= new JwtSecurityTokenHandler().WriteToken(token) };
                }
            }
            return null;
        }

        private JwtSecurityToken GetJwtSecurityToken(User user) 
        {
            var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserId",user.Id.ToString())
                };

            var roles = user.Roles.Split(',').ToList();
            foreach (var role in roles) 
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appConfigurations.JWTConfigurations.Secret));

            var token = new JwtSecurityToken();
            

            return new JwtSecurityToken
            (
                issuer: _appConfigurations.JWTConfigurations.ValidIssuer,
                audience: _appConfigurations.JWTConfigurations.ValidAudience,
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );
        }

    }
}

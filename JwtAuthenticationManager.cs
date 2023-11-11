using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using EgeYurtProject.Models;
using Microsoft.EntityFrameworkCore;
using Azure.Identity;
using EgeYurtProject.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace EgeYurtProject
{
    public class JwtAuthenticationManager : ControllerBase
    {
        //key declaration
        private readonly string key;
        private readonly DbContext _context;

        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        { {"test", "password"}, {"test1", "password1"}, {"user", "password"} };

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
            
        }


        // Creating Token
        public async Task<string> Authenticate(string username, string password)
        {
            if (username == null)
            {
                return null;
            }
            // TODO: GET USER FROM DB
            //user = getUserByUserName(username);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, password)
                }),
                //set duration of token here
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature) //setting sha256 algorithm
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        
    }
}

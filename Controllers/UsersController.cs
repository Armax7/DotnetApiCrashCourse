using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotnetApiCourse.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace dotnetApiCourse.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly IConfiguration configuration;

        public UsersController(UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(UserCredentials userCredentials)
        {
            IdentityUser user = new()
            {
                UserName = userCredentials.Email,
                Email = userCredentials.Email
            };

            IdentityResult result = await userManager.CreateAsync(user, userCredentials.Password);

            if (result.Succeeded)
            {
                return BuildToken(userCredentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private AuthResponse BuildToken(UserCredentials userCredentials)
        {
            //Beware Claims are public
            List<Claim> claims = [
                new Claim("email",userCredentials.Email)
            ];

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(configuration["jwtKey"]));
            SigningCredentials credentials = new(key, SecurityAlgorithms.HmacSha256);

            DateTime expiration = DateTime.UtcNow.AddYears(1);

            JwtSecurityToken securityToken = new(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials
            );

            return new AuthResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }
    }
}
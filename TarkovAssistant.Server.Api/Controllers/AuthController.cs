using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TarkovAssistant.Contracts;
using TarkovAssistant.Domain;
using TarkovAssistant.Server.Services;

namespace TarkovAssistant.Server.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly TokenService _tokenService;

        public AuthController(UserManager<UserEntity> userManager, TokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var user = new UserEntity
            {
                UserName = request.Email,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var token = _tokenService.CreateAccessToken(user);

            return Ok(new
            {
                token = token
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized();

            var valid = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!valid)
                return Unauthorized();

            var accessToken = _tokenService.CreateAccessToken(user);

            var refreshToken = Guid.NewGuid().ToString();

            //_db.RefreshTokens.Add(new RefreshToken
            //{
            //    Token = refreshToken,
            //    UserId = user.Id,
            //    ExpiryDate = DateTime.UtcNow.AddDays(7)
            //});

            //await _db.SaveChangesAsync();

            return Ok(new
            {
                accessToken,
                refreshToken
            });
        }

        //[HttpPost("login")]
        //public IActionResult Login([FromBody] LoginDto model)
        //{
        //    if (model.Username != "admin" || model.Password != "1234")
        //        return Unauthorized();

        //    var claims = new[]
        //    {
        //    new Claim(ClaimTypes.Name, model.Username)
        //};

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddHours(1),
        //        signingCredentials: new SigningCredentials(
        //            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.JwtKey)),
        //            SecurityAlgorithms.HmacSha256)
        //    );

        //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        //    return Ok(new { token = tokenString });
        //}


    }
}

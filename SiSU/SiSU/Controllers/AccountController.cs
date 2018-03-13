using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SiSU.Infrastructure;
using SiSU.Model;
using SiSU.Services;

namespace SiSU.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;

        public AccountController(IUserService userService, IOptions<AppSettings> appSettings) 
        {
            _appSettings = appSettings.Value;
            _userService = userService;
        }


        [Route("login")]
        [HttpPost]
        public IActionResult Login([FromBody]LoginModel model)
        {
            var user = _userService.Authenticate(model.Email, model.Password);
            if (user == null)
            {
                return BadRequest();
            }

            var claims = new List<Claim> {
                new Claim(ClaimTypes.UserData, user.UserId.ToString())
            };
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, ((UserRole)role).ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appSettings.JwtSecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _appSettings.JwtTokenIssuer,
                audience: _appSettings.JwtAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }


        //Below end points are open to allow to create user for testing purposes. 

        [Route("register-referee")]
        [HttpPost]
        public IActionResult RegisterReferee([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.Create(model.Email, model.Password, new List<UserRole> { UserRole.Referee});
                return Ok(new { UserId = user.UserId});
            }
            return NoContent();
        }

        [Route("register-member")]
        [HttpPost]
        public IActionResult RegisterMember([FromBody] RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.Create(model.Email, model.Password, new List<UserRole> { UserRole.Member });
                return Ok(new { UserId = user.UserId });
            }
            return NoContent();
        }
    }
}
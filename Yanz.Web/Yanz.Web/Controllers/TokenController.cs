using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Yanz.DAL.Entities;

namespace Yanz.Web.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        UserManager<AppUser> _userManager;
        SignInManager<AppUser> _signInManager;

        public TokenController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        public async Task<IActionResult> Get()
        {
            var email = Request.Form["email"];
            var password = Request.Form["password"];
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    // проверяем, подтвержден ли email
                    if (!await _userManager.IsEmailConfirmedAsync(user))
                    {
                        ModelState.AddModelError(string.Empty, "You not confirmed your email");
                        return BadRequest();
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync
                                    (user, password, lockoutOnFailure: false);

                    if (!result.Succeeded)
                    {
                        return Unauthorized();
                    }
                    List<Claim> claims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, user.Id) };

                    var role = await _userManager.GetRolesAsync(user);
                    if (role.Count > 0)
                        claims.Add(new Claim(ClaimTypes.Role, role.First()));

                    var token = new JwtSecurityToken
                    (
                        issuer: AuthOptions.ISSUER,
                        audience: AuthOptions.AUDIENCE,
                        claims: claims,
                        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                        notBefore: DateTime.UtcNow,
                        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                    );

                    return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
                }
            }

            return BadRequest();
        }
    }
}
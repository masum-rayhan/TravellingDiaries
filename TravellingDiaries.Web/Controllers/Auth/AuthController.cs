using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravellingDiaries.Models.Auth;

namespace TravellingDiaries.Web.Controllers.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser
            {
                //UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                Surname = model.Surname,
                DateOfBirth = model.DateOfBirth,
                Gender = model.Gender
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                // Generate and return a JWT token
                var token = GenerateJwtToken(user);
                return Ok(new { Token = token });
            }

            return BadRequest(result.Errors);
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user != null)
            {
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, isPersistent: false, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    // User is authenticated; generate and return a JWT token
                    var token = GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }
            }

            // Authentication failed
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");

            return Unauthorized(ModelState);
        }

        return BadRequest(ModelState);
    }

    private string GenerateJwtToken(ApplicationUser user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSettings:Key"));
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            //Subject = new ClaimsIdentity(new[]
            //{
            //    //new Claim(ClaimTypes.NameIdentifier, user.UserName.ToString()),
            //    //new Claim(ClaimTypes.Actor, user.UserName),
            //    //new Claim(ClaimTypes.Name, user.cUserFullname)
            //}),
            Expires = DateTime.UtcNow.AddDays(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

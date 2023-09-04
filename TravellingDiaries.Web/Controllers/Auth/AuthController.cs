using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TravellingDiaries.DataAccess.Repo.IRepo.Auth;
using TravellingDiaries.Models.Auth;

namespace TravellingDiaries.Web.Controllers.Auth;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthRepo _authRepo;

    public AuthController(IAuthRepo authRepo)
    {
        _authRepo = authRepo;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegistrationRequest model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var user = new ApplicationUser
                {
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    Email = model.Email,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                };

                var registeredUser = await _authRepo.RegisterUserAsync(user, model.Password);

                if (registeredUser != null)
                {
                    var token = _authRepo.GenerateJwtToken(registeredUser);
                    return Ok(new { Token = token });
                }

                return BadRequest("User registration failed.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        return BadRequest(ModelState);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        {
            if (ModelState.IsValid)
            {
                var user = await _authRepo.LoginAsync(model.Email, model.Password);

                if (user != null)
                {
                    var token = _authRepo.GenerateJwtToken(user);
                    return Ok(new { Token = token });
                }

                return BadRequest("Invalid login credentials.");
            }

            return BadRequest(ModelState);
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest model)
    {
        if (ModelState.IsValid)
        {
            var result = await _authRepo.ResetPasswordAsync(model.Email, model.NewPassword);

            if (result)
                return Ok("Password reset successful.");

            return BadRequest("Password reset failed.");
        }

        return BadRequest(ModelState);
    }
}

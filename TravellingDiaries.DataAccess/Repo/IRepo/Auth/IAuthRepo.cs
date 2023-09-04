using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingDiaries.Models.Auth;

namespace TravellingDiaries.DataAccess.Repo.IRepo.Auth;

public interface IAuthRepo
{
    Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password);
    Task<ApplicationUser> LoginAsync(string email, string password);
    Task<bool> UserExistsAsync(string email);
    Task<bool> ResetPasswordAsync(string email, string newPassword);
    string GenerateJwtToken(ApplicationUser user);
}

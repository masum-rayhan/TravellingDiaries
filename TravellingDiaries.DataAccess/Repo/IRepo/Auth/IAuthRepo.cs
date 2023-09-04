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
    Task<ApplicationUser> CreateUserAsync(ApplicationUser user, string password);
    Task<ApplicationUser> FindUserByEmailAsync(string email);
    Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
}

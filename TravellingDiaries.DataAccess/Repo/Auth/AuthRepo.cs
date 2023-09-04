using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravellingDiaries.DataAccess.Repo.IRepo.Auth;
using TravellingDiaries.Models.Auth;

namespace TravellingDiaries.DataAccess.Repo.Auth;

public class AuthRepo : IAuthRepo
{
    private readonly UserManager<ApplicationUser> _userManager;
    public AuthRepo(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
    {
        return await _userManager.CreateAsync(user, password);
    }

    public async Task<bool> CheckPasswordAsync(ApplicationUser user, string password)
    {
        return await _userManager.CheckPasswordAsync(user, password);
    }

    public async Task<IdentityUser> FindUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }
}

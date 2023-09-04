﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TravellingDiaries.DataAccess.Data;
using TravellingDiaries.DataAccess.Repo.IRepo.Auth;
using TravellingDiaries.Models.Auth;

namespace TravellingDiaries.DataAccess.Repo.Auth;

public class AuthRepo : IAuthRepo
{
    private readonly IConfiguration _config;
    private readonly AppDbContext _context;

    public AuthRepo(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
    }

    public async Task<ApplicationUser> RegisterUserAsync(ApplicationUser user, string password)
    {
        // Check if a user with the same email exists
        if (await UserExistsAsync(user.Email))
        {
            throw new ApplicationException("Email address is already registered.");
        }

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
        user.PasswordHash = hashedPassword;

        // Add the user to the database
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        //if (result.Succeeded)
        //{
        return user;
        //}
        //else
        //{
        //    throw new ApplicationException("User registration failed. Please check your information.");
        //}
    }

    public async Task<bool> UserExistsAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<ApplicationUser> LoginAsync(string email, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
        {
            return user;
        }

        return null; // Return null for unsuccessful login
    }

    public string GenerateJwtToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Email),
            // Add other claims as needed
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1), // Token expiration time
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}

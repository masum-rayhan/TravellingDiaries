using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingDiaries.Models.Auth;

public class ApplicationUser
{
    [Key]
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Bio { get; set; }
    public string ProfilePicture { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
}

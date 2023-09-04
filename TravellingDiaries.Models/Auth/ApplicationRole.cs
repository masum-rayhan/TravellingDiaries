using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravellingDiaries.Models.Auth;

public class ApplicationRole
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
}

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PessoasInfo.Models;

public class AppUser : IdentityUser
{
    [NotMapped] public string? RoleId { get; set; }
    [NotMapped] public string? Role { get; set; }
    [NotMapped] public IEnumerable<SelectListItem>? RoleList { get; set; }
}
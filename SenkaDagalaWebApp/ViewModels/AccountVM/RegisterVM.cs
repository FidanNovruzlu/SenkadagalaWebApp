using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SenkaDagalaWebApp.ViewModels.AccountVM;

public class RegisterVM
{
    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;
    [MaxLength(25),Required]
    public string UserName { get; set; } = null!;
    [EmailAddress,Required]
    public string Email { get; set; }= null!;
    [DataType(DataType.Password),MinLength(8)]
    public string Password { get; set; } = null!;
    [DataType(DataType.Password),MinLength(8),Compare(nameof(Password))]
    public string ConfrimPassword { get; set; } = null!;

}

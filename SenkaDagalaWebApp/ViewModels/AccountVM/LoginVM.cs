using System.ComponentModel.DataAnnotations;

namespace SenkaDagalaWebApp.ViewModels.AccountVM;

public class LoginVM
{
    public string UserName { get; set; } = null!;
 
    [DataType(DataType.Password), MinLength(8)]
    public string Password { get; set; } = null!;
}

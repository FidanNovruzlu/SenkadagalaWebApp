using SenkaDagalaWebApp.Models;

namespace SenkaDagalaWebApp.ViewModels.ServiceVM;

public class CreateServiceVM
{
    public IFormFile Image { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int JobId { get; set; }
    public List<Job>? Jobs { get; set; }

}

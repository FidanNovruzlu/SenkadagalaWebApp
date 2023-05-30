using SenkaDagalaWebApp.Models;

namespace SenkaDagalaWebApp.ViewModels.ServiceVM;

public class UpdateServiceVM
{
    public IFormFile? Image { get; set; } 
    public string? Description { get; set; }
    public int JobId { get; set; }
    public List<Job>? Jobs { get; set; }
    public string? ImageName    { get; set; }
}

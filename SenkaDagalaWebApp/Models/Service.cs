namespace SenkaDagalaWebApp.Models;

public class Service
{
    public int Id { get; set; }
    public string ImageName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public int JobId { get; set; }
    public Job Job { get; set; } = null!;
}

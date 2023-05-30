namespace SenkaDagalaWebApp.Models;

public class Job
{
    public Job()
    {
        Services = new List<Service>();
    }
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Service> Services { get; set; }
}

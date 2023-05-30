using SenkaDagalaWebApp.Models;

namespace SenkaDagalaWebApp.ViewModels
{
    public class PaginationVM<T>
    {
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
        public List<T> Services { get; set; }
    }
}

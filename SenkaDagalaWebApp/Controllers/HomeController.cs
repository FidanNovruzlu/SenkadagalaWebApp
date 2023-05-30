using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SenkaDagalaWebApp.DAL;
using SenkaDagalaWebApp.Models;
using SenkaDagalaWebApp.ViewModels;
using System.Diagnostics;

namespace SenkaDagalaWebApp.Controllers;
public class HomeController : Controller
{
    private readonly SenkaDbContext _senkaDbContext;

    public HomeController(SenkaDbContext senkaDbContext)
    {
       _senkaDbContext = senkaDbContext;
    }

    public IActionResult Index()
    {
        List<Service> services = _senkaDbContext.Services.Take(4).Include(j=>j.Job).ToList();
        HomeVM homeVM = new HomeVM()
        {
            Services=services
        };
        return View(homeVM);
    }
}
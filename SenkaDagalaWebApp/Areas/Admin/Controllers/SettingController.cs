using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SenkaDagalaWebApp.DAL;
using SenkaDagalaWebApp.Models;
using SenkaDagalaWebApp.ViewModels.SettingVM;
using System.Data;

namespace SenkaDagalaWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SettingController : Controller
{
    private readonly SenkaDbContext _senkaDbContext;
    public SettingController(SenkaDbContext senkaDbContext)
    {
        _senkaDbContext = senkaDbContext;
    }
    public IActionResult Index()
    {
        List<Setting> settings = _senkaDbContext.Settings.ToList();
        return View(settings);
    }
    public IActionResult Update(int id)
    {
        Setting? setting = _senkaDbContext.Settings.FirstOrDefault(x => x.Id == id);
        if (setting == null) return NotFound();

        UpdateSettingVM updateSettingVM = new UpdateSettingVM()
        {
            Value = setting.Value,
        };
        return View(updateSettingVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id,UpdateSettingVM updateSettingVM)
    {
        if (!ModelState.IsValid)
        {
            return View(updateSettingVM);
        }

        Setting? setting = _senkaDbContext.Settings.FirstOrDefault(x => x.Id == id);
        if (setting == null) return NotFound();

        setting.Value = updateSettingVM.Value;

        _senkaDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}

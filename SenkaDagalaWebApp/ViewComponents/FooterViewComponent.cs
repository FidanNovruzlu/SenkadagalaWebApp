using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SenkaDagalaWebApp.DAL;
using SenkaDagalaWebApp.Models;

namespace SenkaDagalaWebApp.ViewComponents;

public class FooterViewComponent:ViewComponent
{

    private readonly SenkaDbContext _senkaDbContext;

	public FooterViewComponent(SenkaDbContext senkaDbContext)
	{
		_senkaDbContext = senkaDbContext;
	}
	public async Task<IViewComponentResult> InvokeAsync()
	{
		Dictionary<string, Setting> settings = await _senkaDbContext.Settings.ToDictionaryAsync(k => k.Key);
		return View(settings);
	}
}

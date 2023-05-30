using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SenkaDagalaWebApp.DAL;
using SenkaDagalaWebApp.Extensions;
using SenkaDagalaWebApp.Models;
using SenkaDagalaWebApp.ViewModels;
using SenkaDagalaWebApp.ViewModels.ServiceVM;
using System.Data;

namespace SenkaDagalaWebApp.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ServiceController : Controller
{
    private readonly SenkaDbContext _senkaDbContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    public ServiceController(SenkaDbContext senkaDbContext,IWebHostEnvironment webHostEnvironment)
    {
        _senkaDbContext = senkaDbContext;
        _webHostEnvironment = webHostEnvironment;
    }
    public IActionResult Index(int page=1, int take=4)
    {
        List<Service> services = _senkaDbContext.Services.Skip((page-1)*take).Take(take).Include(j => j.Job).ToList();

        int allPageCount= _senkaDbContext.Services.Count();
        PaginationVM<Service> paginationVM = new PaginationVM<Service>()
        {
            CurrentPage=page,
            Services= services,
            TotalPage=(int)(Math.Ceiling((double)allPageCount/take))
        };  

        return View(paginationVM);
    }

    public IActionResult Create()
    {
        CreateServiceVM createServiceVM = new CreateServiceVM()
        {
            Jobs = _senkaDbContext.Jobs.ToList()
        };
        return View(createServiceVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceVM createServiceVM)
    {
        if (!ModelState.IsValid)
        {
            createServiceVM.Jobs =await _senkaDbContext.Jobs.ToListAsync();
            return View(createServiceVM);
        }

        if(!createServiceVM.Image.CheckType("image/") & createServiceVM.Image.CheckSize(2048))
        {
            ModelState.AddModelError("", "Incorrect image type or size.");
            createServiceVM.Jobs = _senkaDbContext.Jobs.ToList();
            return View(createServiceVM);
        }
        string newFilename = await createServiceVM.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "img" );
      
        Service service = new Service()
        {
            Description=createServiceVM.Description,
            JobId=createServiceVM.JobId,
            ImageName=newFilename
        };
        _senkaDbContext.Services.Add(service);
        _senkaDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int id)
    {
        Service? service = _senkaDbContext.Services.FirstOrDefault(s => s.Id == id);
        if(service == null)  return NotFound();
  
        UpdateServiceVM updateServiceVM = new UpdateServiceVM()
        {
            Description=service.Description,
            JobId=service.JobId,
            Jobs=_senkaDbContext.Jobs.ToList(),
            ImageName=service.ImageName
        };
        return View(updateServiceVM);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async  Task<IActionResult> Update(int id,UpdateServiceVM updateServiceVM)
    {
        if (!ModelState.IsValid)
        {
            updateServiceVM.Jobs =await _senkaDbContext.Jobs.ToListAsync();
            return View(updateServiceVM);
        }
        Service? service = _senkaDbContext.Services.FirstOrDefault(s => s.Id == id);
        if (service == null) return NotFound();

        if (!updateServiceVM.Image.CheckType("image/") & updateServiceVM.Image.CheckSize(2048))
        {
            ModelState.AddModelError("", "Incorrect image type or size.");
            updateServiceVM.Jobs = _senkaDbContext.Jobs.ToList();
            return View(updateServiceVM);
        }
        if(updateServiceVM.Image != null)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", service.ImageName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            string newFilename = await updateServiceVM.Image.UplaodAsync(_webHostEnvironment.WebRootPath, "img");

            service.ImageName = newFilename;
        }

        service.Description = updateServiceVM.Description;
        service.JobId=updateServiceVM.JobId;
       
        _senkaDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Read(int id)
    {
        Service? service = _senkaDbContext.Services.Include(j=>j.Job).FirstOrDefault(s => s.Id == id);
        if (service == null) return NotFound();
        return View(service);
    }
    [HttpPost]
    public IActionResult Delete(int id)
    {
        Service? service = _senkaDbContext.Services.FirstOrDefault(s => s.Id == id);
        if (service == null) return NotFound();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "img", service.ImageName);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }
        _senkaDbContext.Services.Remove(service);
        _senkaDbContext.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SenkaDagalaWebApp.Models;
using SenkaDagalaWebApp.ViewModels.AccountVM;
using System.Net;
using System.Net.Mail;

namespace SenkaDagalaWebApp.Controllers;

public class AccountController:Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager ,RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager; 
    }
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterVM registerVM)
    {
        if(!ModelState.IsValid) return View(registerVM);
         AppUser user= new AppUser()
         {
             UserName = registerVM.UserName,
             Name=registerVM.Name,
             Email = registerVM.Email,
             Surname = registerVM.Surname,

         };

        IdentityResult identityResult =await _userManager.CreateAsync(user, registerVM.Password);
        if (!identityResult.Succeeded)
        {
            foreach(IdentityError? error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(registerVM);
            }
        }
        IdentityResult result = await _userManager.AddToRoleAsync(user, "Admin");
        if (!result.Succeeded)
        {
            foreach (IdentityError? error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(registerVM);
            }
        }

        string token =await _userManager.GenerateEmailConfirmationTokenAsync(user);
        string link = Url.Action("ConfrimUser", "Account", new {email=registerVM.Email,token=token},HttpContext.Request.Scheme);

        MailMessage message= new MailMessage("7L2P4QW@code.edu.az",user.Email)
        {
            Subject="Confrimation email",
            Body= $"<a href = \"{ link }\"> Click to confrim email.</a>",
            IsBodyHtml=true
        };

        SmtpClient smtpClient = new()
        {
            Host= "smtp.gmail.com",
            Port= 587,
            EnableSsl = true,
            Credentials= new NetworkCredential("7L2P4QW@code.edu.az", "pmretojfjscqqjrk")
        };
        smtpClient.Send(message);

        return RedirectToAction(nameof(Login));
    }
    public async Task< IActionResult> ConfrimUser(string email,string token)
    {
        AppUser user= await  _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

       IdentityResult result=  await  _userManager.ConfirmEmailAsync(user,token);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Incorrect confrim");
            return RedirectToAction("Index", "Home");
        }

         await _signInManager.SignInAsync(user,true);
        return RedirectToAction("Index", "Home");
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM loginVM)
    {
        if (!ModelState.IsValid) return View(loginVM);

        AppUser user =await _userManager.FindByNameAsync(loginVM.UserName);
        if(user == null)
        {
            ModelState.AddModelError("", "Incorrect username or password.");
            return View(loginVM);
        }

        Microsoft.AspNetCore.Identity.SignInResult signInResult= await _signInManager.PasswordSignInAsync(user, loginVM.Password, true, false);
        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("", "Incorrect username or password.");
            return View(loginVM);
        }
        return RedirectToAction("Index","Home");
    }
    [HttpPost]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    #region Create Role
    //public async Task<IActionResult> CreateRole()
    //{
    //    IdentityRole role=new IdentityRole("Admin");
    //    if (role == null) return NotFound();

    //    await _roleManager.CreateAsync(role);
    //    return Json("OK");
    //}
    #endregion
}

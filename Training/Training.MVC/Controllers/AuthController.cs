using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Training.BL.VM.User;
using Training.Core.Entities;
using Training.Core.Enums;

namespace Training.MVC.Controllers;
public class AuthController(SignInManager<User> _signInManager, UserManager<User> _userManager) : Controller
{
    bool isAuthenticated => User.Identity?.IsAuthenticated ?? false;
    public IActionResult Register()
    {
        if (isAuthenticated) return RedirectToAction("Index", "Home");
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVM vm)
    {
        if (isAuthenticated) return RedirectToAction("Index", "Home");
        User user = new User
        {
            Fullname = vm.Username,
            Email = vm.Email,
            UserName = vm.Username, 
        };

        var result = await _userManager.CreateAsync(user, vm.Password);

        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            return View();
        }

        var roleResult = await _userManager.AddToRoleAsync(user, nameof(Roles.User)); 
        if(!roleResult.Succeeded)
        {
            foreach (var error in roleResult.Errors)
                ModelState.AddModelError("", error.Description);
                
            return View();
        }
        return RedirectToAction(nameof(Login));
    }
    public IActionResult Login()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Login(LoginVM vm, string returnUrl = null)
    {
        if (isAuthenticated) return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid) return View();

        User user = null;
        if (vm.UsernameOrEmail.Contains("@"))
            user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);

        else
            user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);

        if (user == null)
        {
            ModelState.AddModelError("", "Username or password is wrong!");
            return View();
        }

        var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.RememberMe, true);

        if (!result.Succeeded)
        {
            if (result.IsNotAllowed)
                ModelState.AddModelError("", "Usernae or Password is wrong");
            if (result.IsLockedOut)
                ModelState.AddModelError("", "Wait until" + user.LockoutEnd!.Value.ToString("yyyy-MM-dd HH:mm:ss"));

            return View();
        }

        if (string.IsNullOrEmpty(returnUrl))
        {
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return RedirectToAction("Index", new { Controller = "Dashboard", Area = "Admin" });

            return RedirectToAction("Index", "Home");
        }

        return LocalRedirect(returnUrl);
    }
    [Authorize]
    public async Task<IActionResult> LogOut()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }
}

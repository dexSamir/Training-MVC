using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Training.MVC.Controllers;
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

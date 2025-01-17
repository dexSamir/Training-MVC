using Microsoft.AspNetCore.Mvc;

namespace Training.MVC.Areas.Admin.Controllers;
[Area("Admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

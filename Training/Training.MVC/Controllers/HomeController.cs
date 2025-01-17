using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Training.DAL.Context;

namespace Training.MVC.Controllers;
public class HomeController(AppdbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Trainers.ToListAsync());
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.BL.Extension;
using Training.BL.VM.Trainer;
using Training.Core.Entities;
using Training.Core.Enums;
using Training.DAL.Context;

namespace Training.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = nameof(Roles.Admin))]
public class TrainerController(AppdbContext _context, IWebHostEnvironment _env) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Trainers.Include(x => x.Category).ToListAsync());
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(TrainerCreateVM vm)
    {
        ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
        if (vm.Image == null || vm.Image.Length == 0)
        {
            ModelState.AddModelError("File", "File is required");
            return View(vm);
        }
        if (!vm.Image.IsValidType("image"))
        {
            ModelState.AddModelError("Image", "File type must be an image");
            return View();
        }
        if (!vm.Image.IsValidSize(5))
        {
            ModelState.AddModelError("Image", "File size must be less than 5MB");
            return View();
        }
        string fileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "trainers");
        Trainer trainer = new Trainer
        {
            ImageUrl = fileName,
            Fullname = vm.Fullname,
            Description = vm.Description,
            CategoryId = vm.CategoryId,
        };
        await _context.Trainers.AddAsync(trainer);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index)); 
    }

    public async Task<IActionResult> Update(int? id)
    {
        ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();

        if (!id.HasValue) return BadRequest();
        var data = await _context.Trainers.Where(x=> x.Id == id)
            .Select(x=> new TrainerUpdateVM
            {
                Fullname = x.Fullname,
                Description = x.Description,
                CategoryId = x.CategoryId,
                ExistingImageUrl = x.ImageUrl
            }).FirstOrDefaultAsync();

        if(data == null) return NotFound(); 
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(int? id, TrainerUpdateVM vm)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Trainers.FindAsync(id);
        if (data == null) return NotFound();

        ViewBag.Categories = await _context.Categories.Where(x => !x.IsDeleted).ToListAsync();
        if(vm.Image != null )
        {
            if (!vm.Image.IsValidType("image"))
            {
                ModelState.AddModelError("Image", "File type must be an image");
                return View(vm);
            }
            if (!vm.Image.IsValidSize(5))
            {
                ModelState.AddModelError("Image", "File size must be less than 5MB");
                return View(vm);
            }
            string fileName = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "imgs", "trainers", data.ImageUrl);
            if (System.IO.File.Exists(fileName))
                System.IO.File.Delete(fileName);

            string newFileName = await vm.Image.UploadAsync(_env.WebRootPath, "imgs", "trainers");
            data.ImageUrl = newFileName;
        } 
        
        data.CategoryId = vm.CategoryId;
        data.Fullname = vm.Fullname;

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> ToggleTrainerVisibility(int? id, bool visible)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Trainers.FindAsync(id);
        if (data == null) return NotFound();
        data.IsDeleted = visible;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Hide(int? id)
        => await ToggleTrainerVisibility(id, true);

    public async Task<IActionResult> Show(int? id)
        => await ToggleTrainerVisibility(id, false);

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Trainers.FindAsync(id);
        if (data == null) return NotFound();

        var filename = Path.Combine(Directory.GetCurrentDirectory(), _env.WebRootPath, "imgs", "categories", data.ImageUrl);

        if(System.IO.File.Exists(filename))
            System.IO.File.Delete(filename);

        _context.Trainers.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

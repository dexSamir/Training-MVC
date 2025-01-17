using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Training.BL.VM.Category;
using Training.Core.Entities;
using Training.Core.Enums;
using Training.DAL.Context;

namespace Training.MVC.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles =nameof(Roles.Admin))]
public class CategoryController(AppdbContext _context) : Controller
{
    public async Task<IActionResult> Index()
    {
        return View(await _context.Categories.ToListAsync());
    }
    public IActionResult Create()
    {
        return View();  
    }
    [HttpPost]
    public async Task<IActionResult> Create(CategoryCrateVM vm)
    {
        if(!ModelState.IsValid) return View();
        var data = await _context.Categories.FirstOrDefaultAsync(x=> x.Name == vm.Name);    
        if(data != null) 
        { 
            ModelState.AddModelError("", "this name is already used");  
        }
        Category category = new Category
        {
            Name = vm.Name,
        };
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();  
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Update(int? id)
    {

        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories
            .Where(x=> x.Id == id)
            .Select(x=> new CategoryUpdateVM
            {
                Name=x.Name,
            }).FirstOrDefaultAsync();
        return View(data);
    }
    [HttpPost]
    public async Task<IActionResult> Update(CategoryUpdateVM vm, int? id)
    {
        if (!id.HasValue) return BadRequest();
        if (!ModelState.IsValid) return View(vm);

        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        data.Name = vm.Name;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> ToggleCategoryVisibility(int? id, bool visible)
    {
        if(!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id); 
        if(data == null ) return NotFound();
        data.IsDeleted = visible;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Hide(int? id) 
        => await ToggleCategoryVisibility(id, true);

    public async Task<IActionResult> Show(int? id)
        => await ToggleCategoryVisibility(id, false);

    public async Task<IActionResult> Delete(int? id)
    {
        if (!id.HasValue) return BadRequest();
        var data = await _context.Categories.FindAsync(id);
        if (data == null) return NotFound();

        _context.Categories.Remove(data);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}

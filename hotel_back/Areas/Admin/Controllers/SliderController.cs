using hotel_back.Areas.Admin.ViewModels.ProductViewModels;
using hotel_back.Areas.Admin.ViewModels.SliderViewModels;
using hotel_back.Contexts;
using hotel_back.Helpers.Extensions;
using hotel_back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]

public class SliderController : Controller
{
    private readonly HotelDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public SliderController(HotelDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders.AsNoTracking().ToListAsync();

        return View(sliders);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SliderCreateViewModel slider)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (!slider.Image.CheckFileType("image/"))
        {
            ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
            return View();
        }

        string fileName = $"{Guid.NewGuid()}-{slider.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await slider.Image.CopyToAsync(stream);
        }

        Slider newSlider = new()
        {
            Description = slider.Description,
            Image = fileName
        };


        await _context.Sliders.AddAsync(newSlider);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detail(int id)
    {
        var slider = await _context.Sliders.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (slider == null)
            return NotFound();

        return View(slider);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider == null)
            return NotFound();

        return View(slider);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeleteSlider(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider == null)
            return NotFound();

        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var slider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (slider == null)
            return NotFound();

        var sliderUpdateViewModel = new SliderUpdateViewModel
        {
            Description = slider.Description,
            Image = null
        };

        return View(sliderUpdateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, SliderUpdateViewModel sliderUpdateViewModel)
    {
        if (!ModelState.IsValid)
            return View(sliderUpdateViewModel);

        var dbSlider = await _context.Sliders.FirstOrDefaultAsync(s => s.Id == id);
        if (dbSlider == null)
            return NotFound();

        if (sliderUpdateViewModel.Image != null)
        {
            if (!sliderUpdateViewModel.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
                return View(sliderUpdateViewModel);
            }

            string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images");
            string path = Path.Combine(basePath, dbSlider.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            string fileName = $"{Guid.NewGuid()}-{sliderUpdateViewModel.Image.FileName}";
            path = Path.Combine(basePath, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await sliderUpdateViewModel.Image.CopyToAsync(stream);
            }
            dbSlider.Image = fileName;
        }

        dbSlider.Description = sliderUpdateViewModel.Description;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

}

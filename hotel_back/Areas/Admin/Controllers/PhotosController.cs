using hotel_back.Areas.Admin.ViewModels.GalleryViewModels;
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
public class PhotosController : Controller
{
    private readonly HotelDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public PhotosController(HotelDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var photos = await _context.Galleries.AsNoTracking().ToListAsync();

        return View(photos);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PhotosCreateViewModel photos)
    {
        if (!ModelState.IsValid)
        {
            return View();
        }
        if (!photos.Image.CheckFileType("image/"))
        {
            ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
            return View();
        }

        string fileName = $"{Guid.NewGuid()}-{photos.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await photos.Image.CopyToAsync(stream);
        }

        Gallery newPhoto = new()
        {
            Image = fileName
        };


        await _context.Galleries.AddAsync(newPhoto);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detail(int id)
    {
        var photo = await _context.Galleries.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (photo == null)
            return NotFound();

        return View(photo);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var photo = await _context.Galleries.FirstOrDefaultAsync(s => s.Id == id);
        if (photo == null)
            return NotFound();

        return View(photo);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeleteSlider(int id)
    {
        var photo = await _context.Galleries.FirstOrDefaultAsync(s => s.Id == id);
        if (photo == null)
            return NotFound();

        _context.Galleries.Remove(photo);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var photo = await _context.Galleries.FirstOrDefaultAsync(s => s.Id == id);
        if (photo == null)
            return NotFound();

        var photosUpdateViewModel = new PhotosUpdateViewModel
        {
            Image = null
        };

        return View(photosUpdateViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, PhotosUpdateViewModel photosUpdateViewModel)
    {
        if (!ModelState.IsValid)
            return View(photosUpdateViewModel);

        var photos = await _context.Galleries.FirstOrDefaultAsync(s => s.Id == id);
        if (photos == null)
            return NotFound();

        if (photosUpdateViewModel.Image != null)
        {
            if (!photosUpdateViewModel.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
                return View(photosUpdateViewModel);
            }

            string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images");
            string path = Path.Combine(basePath, photos.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            string fileName = $"{Guid.NewGuid()}-{photosUpdateViewModel.Image.FileName}";
            path = Path.Combine(basePath, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await photosUpdateViewModel.Image.CopyToAsync(stream);
            }
            photos.Image = fileName;
        }


        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

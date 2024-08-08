using hotel_back.Areas.Admin.ViewModels.ProductViewModels;
using hotel_back.Areas.Admin.ViewModels.ReviewViewModels;
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
public class ReviewController : Controller
{
    private readonly HotelDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ReviewController(HotelDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var review = await _context.Reviews.AsNoTracking().ToListAsync();

        return View(review);
    }

    public async Task<IActionResult> Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReviewCreateViewModel review)
    {

        if (!ModelState.IsValid)
        {
            return View();
        }

        if (!review.Image.CheckFileType("image/"))
        {
            ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
            return View();
        }


        string fileName = $"{Guid.NewGuid()}-{review.Image.FileName}";
        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            await review.Image.CopyToAsync(stream);
        }

        Review newReview= new()
        {
            Name = review.Name,
            Description = review.Description,
            Rating = review.Rating,
            Image = fileName
        };

        await _context.Reviews.AddAsync(newReview);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {
        var review = await _context.Reviews.AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == id);
        if (review == null)
            return NotFound();

        ReviewUpdateViewModel reviewUpdateViewModel = new()
        {
            Name = review.Name,
            Description = review.Description,
            Rating = review.Rating
        };


        return View(reviewUpdateViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, ReviewUpdateViewModel reviewUpdateViewModel)
    {
        if (!ModelState.IsValid)
            return View(reviewUpdateViewModel);

        var review = await _context.Reviews.FirstOrDefaultAsync(s => s.Id == id);
        if (review == null)
            return NotFound();

        if (reviewUpdateViewModel.Image != null)
        {
            if (!reviewUpdateViewModel.Image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "Mutleq shekil olmalidir!!!");
                return View(reviewUpdateViewModel);
            }

            string basePath = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images");
            string path = Path.Combine(basePath, review.Image);

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            string fileName = $"{Guid.NewGuid()}-{reviewUpdateViewModel.Image.FileName}";
            path = Path.Combine(basePath, fileName);

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await reviewUpdateViewModel.Image.CopyToAsync(stream);
            }
            review.Image = fileName;
        }

        review.Name = reviewUpdateViewModel.Name;
        review.Description = reviewUpdateViewModel.Description;
        review.Rating = reviewUpdateViewModel.Rating;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int id)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(s => s.Id == id);
        if (review == null)
            return NotFound();

        return View(review);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeleteReview(int id)
    {
        var review = await _context.Reviews.FirstOrDefaultAsync(s => s.Id == id);
        if (review == null)
            return NotFound();

        _context.Reviews.Remove(review);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Detail(int id)
    {
        var review = await _context.Reviews.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
        if (review == null)
            return NotFound();

        return View(review);
    }
}

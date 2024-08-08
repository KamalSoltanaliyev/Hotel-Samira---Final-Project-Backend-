using hotel_back.Contexts;
using hotel_back.Models;
using hotel_back.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Controllers;

public class ReviewController : Controller
{
    private readonly HotelDbContext _context;
    public ReviewController(HotelDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var reviewUsers = await _context.ReviewUsers
            .Where(r => !r.IsDeleted)
            .Include(r => r.User)
            .ToListAsync();

        var viewModel = new ReviewPageViewModel
        {
            ReviewUsers = reviewUsers
        };

        return View(viewModel);
    }


















    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddReview(string userId, ReviewPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.ReviewUsers = await _context.ReviewUsers
                .Where(r => !r.IsDeleted)
                .Include(r => r.User)
                .ToListAsync();

            return View("Index", model);
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userId);
        if (user == null)
        {
            return NotFound();
        }

        var reviewUser = new ReviewUser
        {
            Rating = model.ReviewForm.Rating,
            Description = model.ReviewForm.Description,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow,
            IsDeleted = false,
            User = user
        };

        _context.ReviewUsers.Add(reviewUser);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }





    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        var reviewUser = await _context.ReviewUsers
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (reviewUser == null || reviewUser.User.UserName != User.Identity.Name)
        {
            return NotFound();
        }

        reviewUser.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [Authorize]
    public async Task<IActionResult> EditReview(int reviewId)
    {
        var reviewUser = await _context.ReviewUsers
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (reviewUser == null || reviewUser.User.UserName != User.Identity.Name)
        {
            return NotFound();
        }

        var reviewFormViewModel = new ReviewFormViewModel
        {
            Description = reviewUser.Description,
            Rating = (int)reviewUser.Rating
        };

        var viewModel = new ReviewPageViewModel
        {
            ReviewForm = reviewFormViewModel,
            ReviewUsers = await _context.ReviewUsers
                .Where(r => !r.IsDeleted)
                .Include(r => r.User)
                .ToListAsync()
        };

        return View("Index", viewModel);
    }





    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateReview(int reviewId, ReviewPageViewModel model)
    {
        var reviewUser = await _context.ReviewUsers
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == reviewId);

        if (reviewUser == null || reviewUser.User.UserName != User.Identity.Name)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            model.ReviewUsers = await _context.ReviewUsers
                .Where(r => !r.IsDeleted)
                .Include(r => r.User)
                .ToListAsync();

            return View("Index", model);
        }

        reviewUser.Description = model.ReviewForm.Description;
        reviewUser.Rating = model.ReviewForm.Rating;
        reviewUser.UpdatedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }


}

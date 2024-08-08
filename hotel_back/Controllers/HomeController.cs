using hotel_back.Contexts;
using hotel_back.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Controllers;

public class HomeController : Controller
{
    private readonly HotelDbContext _context;

    public HomeController(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var sliders = await _context.Sliders.AsNoTracking().ToListAsync();
        var products = await _context.Products.AsNoTracking()
            .Include(p => p.Images)
            .Where(p => !p.IsDeleted).ToListAsync();
        var reviews = await _context.Reviews.AsNoTracking().ToListAsync();
        var category = await _context.Categories.AsNoTracking().ToListAsync();

        HomeViewModel homeViewModel = new HomeViewModel
        {
            Sliders = sliders,
            Products = products,
            Reviews = reviews,
            Categories = category
        };

        return View(homeViewModel);
    }
}

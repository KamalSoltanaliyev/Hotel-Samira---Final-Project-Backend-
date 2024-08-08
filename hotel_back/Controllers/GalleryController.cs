using hotel_back.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Controllers;

public class GalleryController : Controller
{
    private readonly HotelDbContext _context;

    public GalleryController(HotelDbContext context)
    {
        _context = context;
    }
    public async Task<IActionResult> Index()
    {
        var gallery = await _context.Galleries.AsNoTracking().ToListAsync();

        return View(gallery);
    }
}

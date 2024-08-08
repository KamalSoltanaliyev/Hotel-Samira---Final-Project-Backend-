using hotel_back.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class BookingController : Controller
{
    private readonly HotelDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BookingController(HotelDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var bookings = await _context.Bookings
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.User)
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return View(bookings);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var booking = await _context.Bookings
            .AsNoTracking()
            .Include(b => b.User)
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && !b.IsDeleted);

        if (booking == null)
        {
            return NotFound();
        }

        return View(booking);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            return NotFound();
        }

        booking.IsDeleted = true;
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}

using hotel_back.Contexts;
using hotel_back.Helpers;
using hotel_back.Models;
using hotel_back.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Controllers;

public class BookNowController : Controller
{
    private readonly HotelDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public BookNowController(HotelDbContext context, UserManager<AppUser> userManager, IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _configuration = configuration;
        _webHostEnvironment = webHostEnvironment;
    }
    public async Task<IActionResult> Index()
    {
        var category = await _context.Categories.AsNoTracking().ToListAsync();

        HomeViewModel homeViewModel = new HomeViewModel
        {
            Categories = category
        };

        return View(homeViewModel);
    }





    [HttpPost]
    public async Task<IActionResult> Book(BookingViewModel booking)
    {
        if (!ModelState.IsValid)
        {
            return View("Index", new HomeViewModel
            {
                Categories = await _context.Categories.AsNoTracking().ToListAsync()
            });
        }

        var user = _context.Users.FirstOrDefault(u => u.UserName == booking.UserId);
        Booking newBooking = new()
        {
            Name = booking.Name,
            Surname = booking.Surname,
            PhoneNumber = booking.PhoneNumber,
            CheckIn = booking.CheckIn,
            CheckOut = booking.CheckOut,
            Adults = booking.Adults,
            Children = booking.Children,
            Rooms = booking.Rooms,
            CategoryId = booking.CategoryId,
            SpecialRequests = booking.SpecialRequests,
            User = user
        };

        _context.Bookings.Add(newBooking);
        await _context.SaveChangesAsync();

        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "templates", "book-now.html");
        using StreamReader streamReader = new StreamReader(path);
        string content = await streamReader.ReadToEndAsync();
        string body = content
            .Replace("[name]", booking.Name)
            .Replace("[surname]", booking.Surname)
            .Replace("[checkin]", booking.CheckIn)
            .Replace("[checkout]", booking.CheckOut)
            .Replace("[adults]", booking.Adults.ToString())
            .Replace("[children]", booking.Children.ToString())
            .Replace("[rooms]", booking.Rooms.ToString())
            .Replace("[roomtype]", (await _context.Categories.FindAsync(booking.CategoryId)).Name)
            .Replace("[specialrequests]", booking.SpecialRequests);

        EmailHelper emailHelper = new EmailHelper(_configuration);
        await emailHelper.SendEmailAsync(new MailRequest { ToEmail = user.Email, Subject = "Booking Confirmation", Body = body });

        return RedirectToAction(nameof(Index));
    }
}

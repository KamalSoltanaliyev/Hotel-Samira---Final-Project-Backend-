using hotel_back.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.ViewComponents;

public class HeaderViewComponent : ViewComponent
{
    private readonly HotelDbContext _context;

    public HeaderViewComponent(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

        return View(settings);
    }
}

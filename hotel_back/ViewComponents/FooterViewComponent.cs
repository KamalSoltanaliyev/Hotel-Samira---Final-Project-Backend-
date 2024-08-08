using hotel_back.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.ViewComponents;

public class FooterViewComponent : ViewComponent
{
    private readonly HotelDbContext _context;

    public FooterViewComponent(HotelDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var settings = await _context.Settings.ToDictionaryAsync(x => x.Key, x => x.Value);

        return View(settings);
    }
}

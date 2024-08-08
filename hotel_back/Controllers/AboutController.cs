using Microsoft.AspNetCore.Mvc;

namespace hotel_back.Controllers;

public class AboutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

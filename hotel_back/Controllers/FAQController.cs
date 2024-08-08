using Microsoft.AspNetCore.Mvc;

namespace hotel_back.Controllers;

public class FAQController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}

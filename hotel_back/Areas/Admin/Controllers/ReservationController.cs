using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace hotel_back.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class ReservationController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Detail()
    {
        return View();
    }
}

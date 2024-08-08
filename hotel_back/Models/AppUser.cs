using Microsoft.AspNetCore.Identity;

namespace hotel_back.Models;

public class AppUser : IdentityUser
{
    public string Fullname { get; set; }
    public bool IsActive { get; set; }
    ICollection<Comment>? Comments { get; set; }
    ICollection<Booking>? Bookings { get; set; }

    public override string UserName
    {
        get => base.UserName;
        set => base.UserName = value;
    }
}
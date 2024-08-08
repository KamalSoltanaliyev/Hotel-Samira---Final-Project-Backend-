using hotel_back.Models;

namespace hotel_back.ViewModels;

public class HomeViewModel
{
    public List<Slider> Sliders { get; set; }
    public List<Product> Products { get; set; }
    public List<Review> Reviews { get; set; }
    public List<BasketItem> BasketItems { get; set; }
    public List<Category> Categories { get; set; }
    public double TotalPrice { get; set; }
    public List<Booking> Bookings { get; set; }
}

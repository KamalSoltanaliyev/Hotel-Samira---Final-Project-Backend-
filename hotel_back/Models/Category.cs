namespace hotel_back.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public ICollection<Product>? Products { get; set; }
    public ICollection<Booking>? Bookings { get; set; }
}

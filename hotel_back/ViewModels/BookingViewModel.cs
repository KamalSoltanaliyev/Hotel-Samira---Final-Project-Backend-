namespace hotel_back.ViewModels;

public class BookingViewModel
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public string CheckIn { get; set; }
    public string CheckOut { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public int Rooms { get; set; }
    public int CategoryId { get; set; }
    public string SpecialRequests { get; set; }
    public string UserId { get; set; }
}

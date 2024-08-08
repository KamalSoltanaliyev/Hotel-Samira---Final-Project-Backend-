using hotel_back.Models;

namespace hotel_back.ViewModels;

public class ReviewPageViewModel
{
    public List<ReviewUser>? ReviewUsers { get; set; }
    public ReviewFormViewModel ReviewForm { get; set; }
}

namespace hotel_back.Areas.Admin.ViewModels.ReviewViewModels;

public class ReviewUpdateViewModel
{
    public string Name { get; set; }
    public string Description { get; set; }
    public double Rating { get; set; }
    public IFormFile? Image { get; set; }
}

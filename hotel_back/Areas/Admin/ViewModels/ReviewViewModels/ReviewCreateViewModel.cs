using System.ComponentModel.DataAnnotations;

namespace hotel_back.Areas.Admin.ViewModels.ReviewViewModels;

public class ReviewCreateViewModel
{
    [Required]
    public string Name { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public double Rating { get; set; }
    [Required]
    public IFormFile Image { get; set; }
}

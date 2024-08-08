using System.ComponentModel.DataAnnotations;

namespace hotel_back.Areas.Admin.ViewModels.SliderViewModels;

public class SliderCreateViewModel
{
    [Required]
    public string Description { get; set; }
    [Required]
    public IFormFile Image { get; set; }
}

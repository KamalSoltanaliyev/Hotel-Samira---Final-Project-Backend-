using System.ComponentModel.DataAnnotations;

namespace hotel_back.Areas.Admin.ViewModels.GalleryViewModels;

public class PhotosCreateViewModel
{
    [Required]
    public IFormFile Image { get; set; }
}

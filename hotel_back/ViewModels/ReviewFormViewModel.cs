using System.ComponentModel.DataAnnotations;

namespace hotel_back.ViewModels;

public class ReviewFormViewModel
{
    [Required]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }
}

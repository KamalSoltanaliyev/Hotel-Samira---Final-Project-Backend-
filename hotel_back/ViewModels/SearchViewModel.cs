using System.ComponentModel.DataAnnotations;

namespace hotel_back.ViewModels;

public class SearchViewModel
{
    [Required]
    public string Searching { get; set; }
}

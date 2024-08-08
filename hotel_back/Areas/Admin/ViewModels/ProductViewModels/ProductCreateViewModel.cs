using hotel_back.Models;
using System.ComponentModel.DataAnnotations;

namespace hotel_back.Areas.Admin.ViewModels.ProductViewModels;

public class ProductCreateViewModel
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [Required]
    public double Rating { get; set; }
    [Required]
    public double Price { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public int Rooms { get; set; }
    [Required]
    public int Adults { get; set; }
    [Required]
    public int Children { get; set; }
    [Required]
    public string Floor { get; set; }
    [Required]
    public int NumberOfBeds { get; set; }
    [Required]
    public string BedSize { get; set; }
    [Required]
    public double Area { get; set; }
    [Required]
    public DateTime CheckIn { get; set; }
    [Required]
    public DateTime CheckOut { get; set; }
    public ICollection<ProductImage>? ProductImages { get; set; }
    public ICollection<IFormFile>? ProductImage { get; set; }
    public int CategoryId { get; set; }
}

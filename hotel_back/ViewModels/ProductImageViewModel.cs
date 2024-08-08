using hotel_back.Models;

namespace hotel_back.ViewModels;

public class ProductImageViewModel
{
    public List<Product> Products { get; set; }
    public ICollection<ProductImage> ProductImages { get; set; }
    public string SearchItem { get; set; }
}

namespace hotel_back.Models;

public class Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public double Rating { get; set; }
    public double Price { get; set; }
    public string Description { get; set; }
    public int Rooms { get; set; }
    public int Adults { get; set; }
    public int Children { get; set; }
    public string Floor { get; set; }
    public int NumberOfBeds { get; set;}
    public string BedSize { get; set;}
    public double Area { get; set;}
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public ICollection<ProductImage>? Images { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set;}
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public ICollection<Comment>? Comments { get; set; }
}

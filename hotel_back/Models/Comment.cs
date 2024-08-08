namespace hotel_back.Models;

public class Comment
{
    public int Id { get; set; }
    public double Rating { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public bool IsDeleted { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public AppUser User { get; set; }
}

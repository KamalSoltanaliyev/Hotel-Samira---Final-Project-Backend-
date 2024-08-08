using hotel_back.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Contexts;

public class HotelDbContext : IdentityDbContext<AppUser>
{
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

    public DbSet<Slider> Sliders { get; set; } = null!;
    public DbSet<Product> Products { get; set; } = null!;
    public DbSet<ProductImage> ProductImages { get; set; } = null!;
    public DbSet<Review> Reviews { get; set; } = null!;
    public DbSet<Category> Categories { get; set; } = null!;
    public DbSet<Gallery> Galleries { get; set; } = null!;
    public DbSet<Setting> Settings { get; set; } = null!;
    public DbSet<BasketItem> BasketItems { get; set; } = null!;
    public DbSet<Comment> Comments { get; set; } = null!;
    public DbSet<ReviewUser> ReviewUsers { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;
}

using hotel_back.Contexts;
using hotel_back.Models;
using hotel_back.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Controllers;

public class RoomsController : Controller
{
    private readonly HotelDbContext _context;
    private readonly UserManager<AppUser> _userManager;

    public RoomsController(HotelDbContext context, UserManager<AppUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    public async Task<IActionResult> Index()
    {
        var category = await _context.Categories.AsNoTracking().ToListAsync();
        var products = await _context.Products
            .Include(p => p.Images)
            .AsNoTracking()
            .Where(p => !p.IsDeleted)
            .Take(6)
            .ToListAsync();

        List<BasketItem> basketItems = new List<BasketItem>();
        if (User.Identity.IsAuthenticated)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            basketItems = await _context.BasketItems
                .Where(b => b.AppUserId == user.Id)
                .Include(b => b.Product)
                .ThenInclude(p => p.Images)
                .ToListAsync();
        }

        HomeViewModel homeViewModel = new HomeViewModel
        {
            Products = products,
            BasketItems = basketItems,
            Categories = category,
            TotalPrice = basketItems.Sum(b => b.Product.Price)
        };

        return View(homeViewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Filter(DateTime? CheckIn, DateTime? CheckOut, int? Adults, int? Children, int? Rooms, string RoomType)
    {
        var query = _context.Products.AsQueryable();
        query = ApplyFilters(query, CheckIn, CheckOut, Adults, Children, Rooms, RoomType);
        var products = await query.Include(p => p.Images).ToListAsync();

        HomeViewModel homeViewModel = new HomeViewModel
        {
            Products = products,
            Categories = await _context.Categories.AsNoTracking().ToListAsync()
        };

        return View("Filter", homeViewModel);
    }

    private IQueryable<Product> ApplyFilters(IQueryable<Product> query, DateTime? CheckIn, DateTime? CheckOut, int? Adults, int? Children, int? Rooms, string RoomType)
    {
        if (Adults.HasValue)
        {
            query = query.Where(p => p.Adults >= Adults.Value);
        }

        if (Children.HasValue)
        {
            query = query.Where(p => p.Children >= Children.Value);
        }

        if (Rooms.HasValue)
        {
            query = query.Where(p => p.Rooms >= Rooms.Value);
        }

        if (!string.IsNullOrEmpty(RoomType))
        {
            query = query.Where(p => p.Category.Name == RoomType);
        }

        if (CheckIn.HasValue)
        {
            query = query.Where(p => p.UpdatedDate > CheckOut);
        }

        if (CheckOut.HasValue)
        {
            query = query.Where(p => p.UpdatedDate > CheckIn);
        }

        return query;
    }


    [HttpPost]
    public async Task<IActionResult> Search(SearchViewModel model)
    {
        if (ModelState.IsValid)
        {
            var searching = model.Searching.ToLower();
            var filteredProducts = await _context.Products
                .Where(p => p.Title.ToLower().Contains(searching) && !p.IsDeleted)
                .Include(p => p.Images)
                .ToListAsync();

            ProductImageViewModel productImageViewModel = new ProductImageViewModel
            {
                Products = filteredProducts
            };

            return View("Search", productImageViewModel);
        }

        return View("Search", new ProductImageViewModel());
    }

    public async Task<IActionResult> LoadMore(int skip)
    {
        List<Product> products = await _context.Products
            .Where(p => !p.IsDeleted)
            .Skip(skip)
            .Take(6)
            .Include(p => p.Images)
            .ToListAsync();

        return PartialView("_ProductPartial", products);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> AddProductToBasket(int productId)
    {
        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (product == null)
            return Json(new { success = false, message = "Product not found" });

        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null)
            return Json(new { success = false, message = "User not found" });

        var basketItem = await _context.BasketItems
            .Include(b => b.Product)
            .ThenInclude(x => x.Images)
            .FirstOrDefaultAsync(b => b.ProductId == productId && b.AppUserId == user.Id);
        if (basketItem == null)
        {
            BasketItem newBasketItem = new BasketItem
            {
                ProductId = product.Id,
                AppUserId = user.Id,
                Count = 1,
                CreatedDate = DateTime.UtcNow
            };
            await _context.BasketItems.AddAsync(newBasketItem);
        }
        else
        {
            basketItem.Count++;
        }

        await _context.SaveChangesAsync();

        var basketItems = await _context.BasketItems
            .Include(b => b.Product)
            .ThenInclude(x => x.Images)
            .Where(b => b.AppUserId == user.Id)
            .ToListAsync();

        return Json(new { success = true, basketItems = basketItems });
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> RemoveProductFromBasket(int productId)
    {
        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        if (user == null)
            return Json(new { success = false, message = "User not found" });

        var basketItem = await _context.BasketItems
            .FirstOrDefaultAsync(b => b.ProductId == productId && b.AppUserId == user.Id);
        if (basketItem == null)
            return Json(new { success = false, message = "Product not found in basket" });

        if (basketItem.Count > 1)
        {
            basketItem.Count--;
            _context.BasketItems.Update(basketItem);
        }
        else
        {
            _context.BasketItems.Remove(basketItem);
        }
        await _context.SaveChangesAsync();

        return Json(new { success = true, productId = productId });
    }
}

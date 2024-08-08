using hotel_back.Contexts;
using hotel_back.Models;
using hotel_back.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace hotel_back.Controllers;

public class ProductController : Controller
{
    private readonly HotelDbContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(HotelDbContext context, UserManager<AppUser> userManager, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _userManager = userManager;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> ProductDetail(int id)
    {
        var product = await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == id);

        var comments = await _context.Comments
                .Where(c => !c.IsDeleted)
                .Include(c => c.User)
                .ToListAsync();

        if (product == null)
        {
            return NotFound();
        }

        var otherProducts = await _context.Products
            .Where(p => !p.IsDeleted && p.Id != id)
            .Include(p => p.Images)
            .Include(p => p.Comments)
            .ToListAsync();

        var productDetailViewModel = new ProductDetailViewModel
        {
            ProductId = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Rating = product.Rating,
            Rooms = product.Rooms,
            Adults = product.Adults,
            Children = product.Children,
            Floor = product.Floor,
            NumberOfBeds = product.NumberOfBeds,
            BedSize = product.BedSize,
            Area = product.Area,
            CheckIn = product.CheckIn,
            CheckOut = product.CheckOut,
            CategoryId = product.CategoryId,
            Category = product.Category,
            Images = product.Images,
            Comments = product.Comments,
            Products = otherProducts
        };

        var commentFormViewModel = new CommentFormViewModel
        {
            ProductId = product.Id
        };

        var productDetailPageViewModel = new ProductDetailPageViewModel
        {
            ProductDetail = productDetailViewModel,
            CommentForm = new CommentFormViewModel { ProductId = product.Id }
        };

        return View(productDetailPageViewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> AddComment(int productId, string userId, ProductDetailPageViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var comment = new Comment
        {
            ProductId = productId,
            Rating = model.CommentForm.Rating,
            Description = model.CommentForm.Description,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now,
            IsDeleted = false,
            User = _context.Users.FirstOrDefault(u => u.UserName == userId)
        };

        _context.Comments.Add(comment);
        await _context.SaveChangesAsync();

        return RedirectToAction("ProductDetail", new { id = productId });
    }
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> DeleteComment(int commentId)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null || comment.User.UserName != User.Identity.Name)
        {
            return NotFound();
        }

        comment.IsDeleted = true;
        await _context.SaveChangesAsync();

        return RedirectToAction("ProductDetail", new { id = comment.ProductId });
    }




    [Authorize]
    public async Task<IActionResult> EditComment(int commentId)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null || comment.User.UserName != User.Identity.Name)
        {
            return NotFound();
        }

        var commentFormViewModel = new CommentFormViewModel
        {
            ProductId = comment.ProductId,
            Description = comment.Description,
            Rating = (int)comment.Rating
        };

        var product = await _context.Products
            .Where(p => !p.IsDeleted)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Include(p => p.Comments)
            .FirstOrDefaultAsync(p => p.Id == comment.ProductId);

        var productDetailViewModel = new ProductDetailViewModel
        {
            ProductId = product.Id,
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Rating = product.Rating,
            Rooms = product.Rooms,
            Adults = product.Adults,
            Children = product.Children,
            Floor = product.Floor,
            NumberOfBeds = product.NumberOfBeds,
            BedSize = product.BedSize,
            Area = product.Area,
            CheckIn = product.CheckIn,
            CheckOut = product.CheckOut,
            CategoryId = product.CategoryId,
            Category = product.Category,
            Images = product.Images,
            Comments = product.Comments,
            Products = await _context.Products
                .Where(p => !p.IsDeleted && p.Id != product.Id)
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .ToListAsync()
        };

        var productDetailPageViewModel = new ProductDetailPageViewModel
        {
            ProductDetail = productDetailViewModel,
            CommentForm = commentFormViewModel
        };

        return View(productDetailPageViewModel);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdateComment(int commentId, ProductDetailPageViewModel model)
    {
        var comment = await _context.Comments
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == commentId);

        if (comment == null || comment.User.UserName != User.Identity.Name)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var product = await _context.Products
                .Where(p => !p.IsDeleted)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Comments)
                .FirstOrDefaultAsync(p => p.Id == comment.ProductId);

            var productDetailViewModel = new ProductDetailViewModel
            {
                ProductId = product.Id,
                Title = product.Title,
                Description = product.Description,
                Price = product.Price,
                Rating = product.Rating,
                Rooms = product.Rooms,
                Adults = product.Adults,
                Children = product.Children,
                Floor = product.Floor,
                NumberOfBeds = product.NumberOfBeds,
                BedSize = product.BedSize,
                Area = product.Area,
                CheckIn = product.CheckIn,
                CheckOut = product.CheckOut,
                CategoryId = product.CategoryId,
                Category = product.Category,
                Images = product.Images,
                Comments = product.Comments,
                Products = await _context.Products
                    .Where(p => !p.IsDeleted && p.Id != product.Id)
                    .Include(p => p.Images)
                    .Include(p => p.Comments)
                    .ToListAsync()
            };

            var productDetailPageViewModel = new ProductDetailPageViewModel
            {
                ProductDetail = productDetailViewModel,
                CommentForm = model.CommentForm
            };

            return View("EditComment", productDetailPageViewModel);
        }

        comment.Description = model.CommentForm.Description;
        comment.Rating = model.CommentForm.Rating;
        comment.UpdatedDate = DateTime.Now;

        await _context.SaveChangesAsync();

        return RedirectToAction("ProductDetail", "Product", new { id = comment.ProductId });
    }
}


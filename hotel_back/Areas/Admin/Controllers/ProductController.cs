using hotel_back.Areas.Admin.ViewModels.ProductViewModels;
using hotel_back.Contexts;
using hotel_back.Helpers.Extensions;
using hotel_back.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace hotel_back.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin,Moderator")]
public class ProductController : Controller
{
    private readonly HotelDbContext _context;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(HotelDbContext context, IWebHostEnvironment webHostEnvironment)
    {
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => !p.IsDeleted)
            .ToListAsync();

        return View(products);
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel product)
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();

        if (!ModelState.IsValid)
        {
            return View();
        }


        foreach (var image in product.ProductImage)
        {
            if (!image.CheckFileType("image/"))
            {
                ModelState.AddModelError("Image", "There must be an image!!!");
                return View();
            }
        }

        List<ProductImage> productImages = new();
        foreach (var image in product.ProductImage)
        {
            string fileName = $"{Guid.NewGuid()}-{image.FileName}";
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            ProductImage productImage = new()
            {
                Image = fileName,
                ProductId = product.Id
            };

            productImages.Add(productImage);
        }


        Product newProduct = new()
        {
            Title = product.Title,
            Description = product.Description,
            Price = product.Price,
            Rating = product.Rating,
            //Image = fileName,
            Images = productImages,
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
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        await _context.Products.AddAsync(newProduct);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Update(int id)
    {

        var product = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (product == null)
            return NotFound();

        ProductUpdateViewModel productUpdateViewModel = new()
        {
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
            ProductImages = product.Images
        };

        ViewBag.Categories = await _context.Categories.ToListAsync();

        return View(productUpdateViewModel);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id, ProductUpdateViewModel productUpdateViewModel)
    {
        TempData["ReturnUrl"] = Request.Headers["Referer"].ToString();

        ViewBag.Categories = await _context.Categories.ToListAsync();

        if (!ModelState.IsValid)
            return View();

        var product = await _context.Products
            .Include(p => p.Images)
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

        if (product == null) 
            return NotFound();

        List<ProductImage> ProductImages = new();
        foreach (var image in product.Images)
        {
            ProductImages.Add(image);
        }

        if (productUpdateViewModel.ProductImage != null)
        {
            foreach (var image in productUpdateViewModel.ProductImage)
            {
                if (!image.CheckFileType("image/"))
                {
                    ModelState.AddModelError("Image", "There must be an image!!!");
                    return View();
                }

                string fileName = $"{Guid.NewGuid()}-{image.FileName}";
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", fileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }

                ProductImage productImage = new()
                {
                    Image = fileName,
                    ProductId = product.Id
                };

                ProductImages.Add(productImage);
            }      
        }

        product.Title = productUpdateViewModel.Title;
        product.Description = productUpdateViewModel.Description;
        product.Price = productUpdateViewModel.Price;
        product.Rating = productUpdateViewModel.Rating;
        product.Rooms = productUpdateViewModel.Rooms;
        product.Adults = productUpdateViewModel.Adults;
        product.Children = productUpdateViewModel.Children;
        product.Floor = productUpdateViewModel.Floor;
        product.NumberOfBeds = productUpdateViewModel.NumberOfBeds;
        product.BedSize = productUpdateViewModel.BedSize;
        product.Area = productUpdateViewModel.Area;
        product.CheckIn = productUpdateViewModel.CheckIn;
        product.CheckOut = productUpdateViewModel.CheckOut;
        product.UpdatedDate = DateTime.UtcNow;
        product.Images = ProductImages;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var product = await _context.Products
            .AsNoTracking()
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (product == null) 
            return NotFound();

        return View(product);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName(nameof(Delete))]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
        if (product == null)
            return NotFound();

        foreach (var image in product.Images)
        {
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", image.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
        }

        product.IsDeleted = true;

        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detail(int id)
    {
        var product = await _context.Products.AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(s => s.Id == id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    public async Task<IActionResult> DeleteImage(int id )
    {
        var returnUrl = TempData["ReturnUrl"] as string;
        var image = await _context.ProductImages.FirstOrDefaultAsync(i => i.Id == id);

        if (image == null)
            return NotFound();


        string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "images", image.Image);
        if (System.IO.File.Exists(path))
        {
            System.IO.File.Delete(path);
        }

        _context.ProductImages.Remove(image);
        await _context.SaveChangesAsync();

        
            return Redirect(returnUrl);
        

    }
}

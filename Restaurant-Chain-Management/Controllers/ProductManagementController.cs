using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Chain_Management.DTOs;
using Restaurant_Chain_Management.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Restaurant_Chain_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "GeneralManager,BranchManager,Admin")]
    public class ProductManagementController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProductManagementController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet("ShowAllProducts")]
        public GeneralResponse Index(int stockId)
        {
            var products = context.Branches
                .Where(b => b.StockId == stockId)
                .Include(b => b.Stock)
                    .ThenInclude(s => s.StockProducts)
                        .ThenInclude(sp => sp.Product)
                .SelectMany(b =>
                b.Stock.StockProducts.Select(sp => new
                {
                    sp.Product.Id,
                    sp.Product.Name,
                    sp.Product.Price,
                    Images = sp.Product.ImageProducts.Select(ip => ip.ImageUrl),
                    //sp.Product.IsFavorite,
                    sp.Quantity,
                    ManagerName = b.Manager != null ? b.Manager.Name : null
                }))
                .ToList();

            if (products.Any())
            {
                return new GeneralResponse
                {
                    IsSuccess = true,
                    Data = products
                };
            }
            return new GeneralResponse
            {
                IsSuccess = false,
                Data = "No products found for the specified stock."
            };
        }

        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct([FromForm] ProductDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                Des = dto.Des,
                Price = dto.Price,
                //IsFavorite = dto.IsAvailable
                GlobalCode = Guid.NewGuid().ToString("N")
            };

            await context.Products.AddAsync(product);
            await context.SaveChangesAsync();

            var stockProduct = new StockProduct
            {
                StockId = dto.StockId,
                ProductId = product.Id,
                Quantity = dto.Quantity
            };
            await context.StockProducts.AddAsync(stockProduct);
            await context.SaveChangesAsync();

            // رفع الصور
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            foreach (var file in dto.Images)
            {
                var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(folderPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                var imageProduct = new ImageProduct
                {
                    ImageUrl = $"/uploads/{fileName}", // مسار الصورة بالنسبة للـ client
                    ProductId = product.Id
                };

                await context.ImageProducts.AddAsync(imageProduct);
            }

            await context.SaveChangesAsync();


            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = new
                {
                    product.Id,
                    product.Name,
                    product.Des,
                    product.Price,
                    dto.Quantity,
                    dto.StockId
                }
            });
        }

        [HttpPut("UpdateProduct/{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromForm] UpdateProdcutDto dto)
        {
            var product = await context.Products.FindAsync(id);
            if (product == null) return NotFound("Product not found");

            //Update data only if sent
            product.Name = !string.IsNullOrWhiteSpace(dto.Name) ? dto.Name : product.Name;
            product.Des = !string.IsNullOrWhiteSpace(dto.Des) ? dto.Des : product.Des;
            product.Price = dto.Price.HasValue ? dto.Price.Value : product.Price;
            //product.IsFavorite = dto.IsAvailable.HasValue ? dto.IsAvailable.Value : product.IsFavorite;

            //  Update inventory if StockId and Quantity are present
            if (dto.StockId.HasValue && dto.Quantity.HasValue)
            {
                var stockProduct = await context.StockProducts
                    .FirstOrDefaultAsync(sp => sp.ProductId == id && sp.StockId == dto.StockId.Value);

                if (stockProduct != null)
                {
                    stockProduct.Quantity = dto.Quantity.Value;
                }
            }

            //  Update Favorites
            //var favorite = await context.FavoriteProducts.FirstOrDefaultAsync(f => f.ProductId == id);

            //if (product.IsFavorite)
            //{
            //    if (favorite == null)
            //    {
            //        var favProduct = new FavoriteProduct
            //        {
            //            ProductId = id,
            //            AddedDate = DateTime.Now
            //        };
            //        await context.FavoriteProducts.AddAsync(favProduct);
            //    }
            //}
            //else
            //{
            //    if (favorite != null)
            //    {
            //        context.FavoriteProducts.Remove(favorite);
            //    }
            //}

            //  Update photos (if new photos are sent)
            if (dto.Images != null && dto.Images.Count > 0)
            {
                // احذف القديمة
                var oldImages = context.ImageProducts.Where(ip => ip.ProductId == id).ToList();
                foreach (var img in oldImages)
                {
                    var oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", img.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldPath))
                        System.IO.File.Delete(oldPath);

                    context.ImageProducts.Remove(img);
                }

                // اضف الجديدة
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                foreach (var file in dto.Images)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var imageProduct = new ImageProduct
                    {
                        ImageUrl = $"/uploads/{fileName}",
                        ProductId = product.Id
                    };

                    await context.ImageProducts.AddAsync(imageProduct);
                }
            }

            await context.SaveChangesAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = new
                {
                    product.Id,
                    product.Name,
                    product.Des,
                    product.Price,
                    //product.IsFavorite,
                    dto.Quantity
                }
            });
        }


        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await context.Products
                .Include(p => p.StockProducts)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null) return NotFound("Product not found");

            // Clear the first relationship (StockProduct)
            context.StockProducts.RemoveRange(product.StockProducts);

            //Then the product itself
            context.Products.Remove(product);
            await context.SaveChangesAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = $"Product {product.Name} deleted successfully."
            });
        }

    }
}

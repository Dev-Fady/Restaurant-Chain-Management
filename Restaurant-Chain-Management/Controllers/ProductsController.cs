using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Restaurant_Chain_Management.DTOs;
using Restaurant_Chain_Management.Models;
using System.Security.Claims;

namespace Restaurant_Chain_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext context;

        public ProductsController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet("ShowAllProducts")]
        public async Task<IActionResult> ShowAllProducts(int stockId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var products = await context.StockProducts
                .Where(sp => sp.StockId == stockId)
                .Include(CI => CI.Product.CartItems)
                .Select(sp => new ProductResponseDTO
                {
                    Id = sp.Product.Id,
                    GlobalCode = sp.Product.GlobalCode,
                    Name = sp.Product.Name,
                    Price = sp.Product.Price,
                    Des = sp.Product.Des,
                    Images = sp.Product.ImageProducts.Select(ip => ip.ImageUrl),
                    IsFavorite = context.FavoriteProducts
                        .Any(fp =>
                        fp.ProductId == sp.Product.Id && fp.UserId == userId),
                    IsCart = sp.Product.CartItems
                            .Any(ci => ci.UserId == userId),
                    CartQuantity = sp.Product.CartItems
                            .Where(ci => ci.UserId == userId)
                            .Select(ci => (int?)ci.Quantity)
                            .FirstOrDefault() ?? 0
                })
                .ToListAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = products
            });
        }

        [HttpPost("AddFavorite/{globalCode}")]
        public async Task<IActionResult> AddFavorite(string globalCode)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Success = false, Message = "User not authenticated." });
            }

            var stockProduct = await context.StockProducts
                .Include(sp => sp.Product)
                .FirstOrDefaultAsync(sp => sp.Product.GlobalCode == globalCode);

            if (stockProduct == null)
            {
                return NotFound(new { Success = false, Message = "Product not found in stock." });
            }

            var exists = await context.FavoriteProducts
                .AnyAsync(f => f.stockId == stockProduct.StockId
                               && f.ProductId == stockProduct.ProductId
                               && f.UserId == userId);

            if (exists)
            {
                return BadRequest(new { Success = false, Message = "Product already in favorites." });
            }

            await context.FavoriteProducts.AddAsync(new FavoriteProduct
            {
                ProductId = stockProduct.ProductId,
                UserId = userId,
                stockId = stockProduct.StockId,
                AddedDate = DateTime.Now
            });

            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Product added to favorites." });
        }

        [HttpPost("AddToCart/{globalCode}")]
        public async Task<IActionResult> AddToCart(string globalCode, int quantity = 1)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Success = false, Message = "User not authenticated." });
            }

            var stockProduct = await context.StockProducts
                .Include(sp => sp.Product)
                .FirstOrDefaultAsync(sp => sp.Product.GlobalCode == globalCode);

            if (stockProduct == null)
            {
                return NotFound(new { Success = false, Message = "Product not found in stock." });
            }

            if (quantity > stockProduct.Quantity)
            {
                return BadRequest(new { Success = false, Message = "Not enough stock available." });
            }

            var cartItem = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.ProductId == stockProduct.ProductId
                                        && ci.stockId == stockProduct.StockId
                                        && ci.UserId == userId);

            if (cartItem != null)
            {
                int newQuantity = cartItem.Quantity + quantity;
                if (newQuantity > stockProduct.Quantity)
                {
                    return BadRequest(new { Success = false, Message = "Not enough stock available." });
                }
                cartItem.Quantity = newQuantity;
                context.CartItems.Update(cartItem);
            }
            else
            {
                await context.CartItems.AddAsync(new CartItem
                {
                    ProductId = stockProduct.ProductId,
                    UserId = userId,
                    Quantity = quantity,
                    stockId = stockProduct.StockId,
                    AddedDate = DateTime.Now
                });
            }

            stockProduct.Quantity -= quantity;
            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Product added to cart." });
        }

        [HttpDelete("RemoveFavorite/{globalCode}")]
        public async Task<IActionResult> RemoveFavorite(string globalCode)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var stockProduct = await context.StockProducts
                .Include(sp => sp.Product)
                .FirstOrDefaultAsync(sp => sp.Product.GlobalCode == globalCode);

            if (stockProduct == null)
            {
                return NotFound(new { Success = false, Message = "Product not found in stock." });
            }

            var fav = await context.FavoriteProducts
                .FirstOrDefaultAsync(f => f.stockId == stockProduct.StockId
                                       && f.ProductId == stockProduct.ProductId
                                       && f.UserId == userId);

            if (fav == null)
            {
                return NotFound(new { Success = false, Message = "Product not found in favorites." });
            }

            context.FavoriteProducts.Remove(fav);
            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Product removed from favorites." });
        }

        [HttpDelete("RemoveFromCart/{globalCode}")]
        public async Task<IActionResult> RemoveFromCart(string globalCode)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var stockProduct = await context.StockProducts
                .Include(sp => sp.Product)
                .FirstOrDefaultAsync(sp => sp.Product.GlobalCode == globalCode);

            if (stockProduct == null)
            {
                return NotFound(new { Success = false, Message = "Product not found in stock." });
            }

            var cartItem = await context.CartItems
                .FirstOrDefaultAsync(ci => ci.stockId == stockProduct.StockId
                                        && ci.ProductId == stockProduct.ProductId
                                        && ci.UserId == userId);

            if (cartItem == null)
            {
                return NotFound(new { Success = false, Message = "Product not found in cart." });
            }

            stockProduct.Quantity += cartItem.Quantity;

            context.CartItems.Remove(cartItem);
            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = "Product removed from cart." });
        }

    }
}

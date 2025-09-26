using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Restaurant_Chain_Management.DTOs;
using Restaurant_Chain_Management.Models;

namespace Restaurant_Chain_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "BranchManager,GeneralManager,Admin")]
    public class OfferProManagementController : ControllerBase
    {
        private readonly AppDbContext context;

        public OfferProManagementController(AppDbContext context)
        {
            this.context = context;
        }
        [HttpGet("AllBranchOffers")]
        public async Task<GeneralResponse> AllBranchOffers()
        {
            var offers =await context.OfferStocks
                .Include(os => os.Offer)
                .ThenInclude(o => o.Images)
                .Select(
                os => new
                {
                    OfferId = os.Offer.Id,
                    BranchId = os.BranchId,
                    BranchName=os.Branch.Name,
                    OfferName = os.Offer.Name,
                    OfferDescription = os.Offer.Des,
                    OfferPrice = os.Offer.Price,
                    StartDate = os.Offer.StartDate,
                    EndDate = os.Offer.EndDate,
                    Images = os.Offer.Images.Select(i => i.ImageUrl).ToList(),
                    Quantity = os.Quantity
                })
                .ToListAsync();
            if (!offers.Any())
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "No offers found for this branch."
                };
            }
            return new GeneralResponse
            {
                IsSuccess = true,
                Data = offers
            };
        }

        [HttpGet("GetBranchOffers/{branchId}")]
        public async Task<GeneralResponse> GetBranchOffers(int branchId)
        {
            var offers = await context.OfferStocks
                .Where(os => os.BranchId == branchId)
                .Include(os => os.Offer)
                .ThenInclude(o => o.Images)
                .Select(
                os => new
                {
                    OfferId = os.Offer.Id,
                    OfferName = os.Offer.Name,
                    BranchId = os.BranchId,
                    BranchName = os.Branch.Name,
                    OfferDescription = os.Offer.Des,
                    OfferPrice = os.Offer.Price,
                    StartDate = os.Offer.StartDate,
                    EndDate = os.Offer.EndDate,
                    Images = os.Offer.Images.Select(i => i.ImageUrl).ToList(),
                    Quantity = os.Quantity
                })
                .ToListAsync();
            if (!offers.Any())
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "No offers found for this branch."
                };
            }
            return new GeneralResponse
            {
                IsSuccess = true,
                Data = offers
            };
        }

        [HttpPost("AddOfer")]
        public async Task<GeneralResponse> AddOffer([FromForm] AddOfferDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = ModelState
                };
            }
            var newoffer = new Offer
            {
                Name = dto.Name,
                Des = dto.Description,
                Price = dto.Price,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
            };
            context.Offers.Add(newoffer);
            await context.SaveChangesAsync();

            // رفع الصور
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfferImage");
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

                var imageOffer = new ImageOffer
                {
                    ImageUrl = $"/uploads/{fileName}",
                    OfferId = newoffer.Id
                };

                await context.ImageOffers.AddAsync(imageOffer);
            }
            {
                var newOfferStock = new OfferStock
                    {
                        OfferId = newoffer.Id,
                        BranchId = dto.OfferStocksJson.BranchId,
                        Quantity = dto.OfferStocksJson.Quantity,
                        Name = $"{dto.OfferStocksJson.Name} - {newoffer.Name}"
                    };
                await context.OfferStocks.AddAsync(newOfferStock);
            }
            await context.SaveChangesAsync();

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = "Offer added successfully."
            };
        }

        [HttpDelete("DeleteOffer/{id}")]
        public async Task<GeneralResponse> DeleteOffer(int id)
        {
            var offer = await context.Offers.FindAsync(id);
            if (offer == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Offer not found."
                };
            }

            var images = context.ImageOffers.Where(i => i.OfferId == offer.Id).ToList();
            context.ImageOffers.RemoveRange(images);

            var stocks = context.OfferStocks.Where(s => s.OfferId == offer.Id).ToList();
            context.OfferStocks.RemoveRange(stocks);

            context.Offers.Remove(offer);

            await context.SaveChangesAsync();

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = "Offer deleted successfully."
            };
        }

        [HttpPut("UpdateOffer/{id}")]
        public async Task<GeneralResponse> UpdateOffer(int id, [FromForm] AddOfferDTO dto)
        {
            var offer = await context.Offers.FindAsync(id);
            if (offer == null)
            {
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Offer not found."
                };
            }

            offer.Name = string.IsNullOrEmpty(dto.Name) ? offer.Name : dto.Name;
            offer.Des = string.IsNullOrEmpty(dto.Description) ? offer.Des : dto.Description;
            offer.Price = dto.Price != 0 ? dto.Price : offer.Price;
            offer.StartDate = dto.StartDate != default ? dto.StartDate : offer.StartDate;
            offer.EndDate = dto.EndDate != default ? dto.EndDate : offer.EndDate;

            if (dto.Images != null && dto.Images.Any())
            {
                var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "OfferImage");
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

                    var imageOffer = new ImageOffer
                    {
                        ImageUrl = $"/uploads/{fileName}",
                        OfferId = offer.Id
                    };

                    await context.ImageOffers.AddAsync(imageOffer);
                }
            }

            if (dto.OfferStocksJson != null)
            {
                var existingStock = await context.OfferStocks
                    .FirstOrDefaultAsync(s => s.OfferId == offer.Id && s.BranchId == dto.OfferStocksJson.BranchId);

                if (existingStock != null)
                {
                    existingStock.Quantity = dto.OfferStocksJson.Quantity != 0 ? dto.OfferStocksJson.Quantity : existingStock.Quantity;
                    existingStock.Name = string.IsNullOrEmpty(dto.OfferStocksJson.Name) ? existingStock.Name : $"{dto.OfferStocksJson.Name} - {offer.Name}";
                }
                else
                {
                    var newStock = new OfferStock
                    {
                        OfferId = offer.Id,
                        BranchId = dto.OfferStocksJson.BranchId,
                        Quantity = dto.OfferStocksJson.Quantity,
                        Name = $"{dto.OfferStocksJson.Name} - {offer.Name}"
                    };
                    await context.OfferStocks.AddAsync(newStock);
                }
            }

            await context.SaveChangesAsync();

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = "Offer updated successfully."
            };
        }

    }
}

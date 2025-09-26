using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using Restaurant_Chain_Management.DTOs;
using Restaurant_Chain_Management.Models;
using Restaurant_Chain_Management.Models.Enums;
using System.Data;

namespace Restaurant_Chain_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "GeneralManager,Admin")]
    public class BranchesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BranchesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetBranches")]
        public async Task<ActionResult<GeneralResponse>> GetBranches()
        {
            var branches = await _context.Branches
                .Include(b => b.City)
                .Include(b => b.Manager)
                .Include(b => b.Stock)
                .Select(b => new
                {
                    b.Id,
                    b.Name,
                    City = new
                    {
                        b.City.Id,
                        b.City.Name
                    },
                    Manager = b.Manager != null ? new ManagerDto
                    {
                        Id = b.Manager.Id,
                        Name = b.Manager.Name,
                        RoleId = (int?)b.Manager.Role,
                    } : null,
                    Stock = new
                    {
                        b.Stock.Id,
                        b.Stock.Name,
                        b.Stock.StockProducts.Count
                    }
                })
                .ToListAsync();

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = branches
            };
        }

        [HttpGet("GetBranch/{id}")]
        public async Task<ActionResult<object>> GetBranch(int id)
        {
            var branch = await _context.Branches
                .Include(b => b.City)
                .Include(b => b.Manager)
                .Include(b => b.Stock)
                .Where(b => b.Id == id)
                .Select(b => new
                {
                    b.Id,
                    b.Name,
                    City = new
                    {
                        b.City.Id,
                        b.City.Name
                    },
                    Manager = b.Manager != null ? new ManagerDto
                    {
                        Id = b.Manager.Id,
                        Name = b.Manager.Name,
                        RoleId = (int?)b.Manager.Role
                    } : null,
                    Stock = new
                    {
                        b.Stock.Id,
                        b.Stock.Name,
                        b.Stock.StockProducts.Count
                    }
                })
                .FirstOrDefaultAsync();

            if (branch == null)
                return new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "No products found for the specified stock."
                };

            return new GeneralResponse
            {
                IsSuccess = true,
                Data = branch
            };

        }

        // POST: api/Branches
        [HttpPost("CreateBranch")]
        public async Task<ActionResult> CreateBranch(BranchDTO branch)
        {
            if (branch == null)
            {
                return BadRequest(new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Invalid branch data."
                });
            }

            var city = await _context.Cities.FindAsync(branch.CityId);
            if (city == null)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "City not found."
                });
            }

            //Employee? manager = null;
            //if (branch.ManagerId.HasValue)
            //{
            //    manager = await _context.Employees.FindAsync(branch.ManagerId.Value);
            //    if (manager == null || manager.Role != EmployeeRole.BranchManager)
            //    {
            //        return BadRequest(new GeneralResponse
            //        {
            //            IsSuccess = false,
            //            Data = "Invalid manager."
            //        });
            //    }
            //}

            var newBranch = new Branch
            {
                Name = branch.Name,
                CityId = branch.CityId,
                //ManagerId = branch.ManagerId,
            };

            _context.Branches.Add(newBranch);
            await _context.SaveChangesAsync();

            // Automatically create Stock linked to this Branch
            var stock = new Stock
            {
                Name = $"{branch.Name} Stock",
                BranchId = newBranch.Id
            };

            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            // Update branch with StockId
            newBranch.StockId = stock.Id;
            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = new
                {
                    newBranch.Id,
                    newBranch.Name,
                    City = city.Name,
                    //Manager = manager?.Name,
                    Stock = stock.Name
                }
            });
        }

        [HttpPost("CreateCity")]
        public async Task<ActionResult> CreateCity(City city)
        {
            if (city == null)
            {
                return BadRequest(new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Invalid city data."
                });
            }
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();
            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = city
            });
        }

      /*  //// DELETE: api/Cities/DeleteCity/5
        //[HttpDelete("DeleteCity/{id}")]
        //public async Task<ActionResult> DeleteCity(int id)
        //{
        //    var city = await _context.Cities
        //        .Include(c => c.Branches)
        //        .FirstOrDefaultAsync(c => c.Id == id);

        //    if (city == null)
        //    {
        //        return NotFound(new GeneralResponse
        //        {
        //            IsSuccess = false,
        //            Data = "City not found."
        //        });
        //    }

        //    if (city.Branches.Any())
        //    {
        //        return BadRequest(new GeneralResponse
        //        {
        //            IsSuccess = false,
        //            Data = $"Cannot delete city '{city.Name}' because it has related branches."
        //        });
        //    }

        //    _context.Cities.Remove(city);
        //    await _context.SaveChangesAsync();

        //    return Ok(new GeneralResponse
        //    {
        //        IsSuccess = true,
        //        Data = $"City {city.Name} deleted successfully."
        //    });
        //}
        */

        // PUT: api/Cities/UpdateCity/5  
        [HttpPut("UpdateCity/{id}")]
        public async Task<ActionResult> UpdateCity(int id, City cityDto)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "City not found."
                });
            }

            city.Name = cityDto.Name;
            _context.Cities.Update(city);
            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = city
            });
        }

        // DELETE: api/Branches/DeleteBranch/5
        [HttpDelete("DeleteBranch/{id}")]
        public async Task<ActionResult> DeleteBranch(int id)
        {
            var branch = await _context.Branches
                .Include(b => b.Stock)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (branch == null)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Branch not found."
                });
            }

            // Delete the associated store (optional if the relationship is Required)
            if (branch.Stock != null)
            {
                _context.Stocks.Remove(branch.Stock);
            }

            _context.Branches.Remove(branch);
            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = $"Branch {branch.Name} deleted successfully."
            });
        }

        // PUT: api/Branches/UpdateBranch/5
        [HttpPut("UpdateBranch/{id}")]
        public async Task<ActionResult> UpdateBranch(int id, BranchDTO branchDto)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
            {
                return NotFound(new GeneralResponse
                {
                    IsSuccess = false,
                    Data = "Branch not found."
                });
            }

            if (!string.IsNullOrEmpty(branchDto.Name))
                branch.Name = branchDto.Name;

            if (branchDto.CityId != 0)
            {
                var city = await _context.Cities.FindAsync(branchDto.CityId);
                if (city == null)
                {
                    return NotFound(new GeneralResponse
                    {
                        IsSuccess = false,
                        Data = "City not found."
                    });
                }
                branch.CityId = branchDto.CityId;
            }

            //if (branchDto.ManagerId.HasValue && branchDto.ManagerId.Value > 0)
            //{
            //    var manager = await _context.Employees.FindAsync(branchDto.ManagerId.Value);
            //    if (manager == null || manager.Role != EmployeeRole.BranchManager)
            //    {
            //        return BadRequest(new GeneralResponse
            //        {
            //            IsSuccess = false,
            //            Data = "Invalid manager."
            //        });
            //    }
            //    branch.ManagerId = branchDto.ManagerId;
            //}

            await _context.SaveChangesAsync();

            return Ok(new GeneralResponse
            {
                IsSuccess = true,
                Data = new
                {
                    branch.Id,
                    branch.Name,
                    branch.CityId,
                    branch.ManagerId,
                    branch.StockId
                }
            });
        }


    }
}

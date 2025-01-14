using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.Models;
using crombie_ecommerce.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.Models.Dto;

namespace crombie_ecommerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly WishlistService _wishlistService;

        public WishlistController(WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        // Get all wishlists
        [HttpGet]
        public async Task<ActionResult<List<Wishlist>>> GetAllWishlists()
        {
            var wishlists = await _wishlistService.GetAllWishlists();
            return Ok(new {message = "All wishlists retrieved succesfully.", wishlists});
        }

        // Get wishlist by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Wishlist>> GetWishlistById(Guid id)
        {
            var wishlist = await _wishlistService.GetWishlistById(id);
            if (wishlist == null)
            {
                return NotFound(new { message = "Wishlist not found." });
            }
            return Ok(new { message = "Wishlist retrieved successfully.", wishlist});
        }

        // Create a new wishlist
        [HttpPost]
        public async Task<IActionResult> CreateWishlist([FromBody] WishlistDto wishlistDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var wishlist = new Wishlist
                {
                    WishlistId = Guid.NewGuid(),
                    Name = wishlistDto.Name,
                    Description = wishlistDto.Description,
                    UserId = wishlistDto.UserId
                };
                var createdWishlist = await _wishlistService.CreateWishlist(wishlist);
                return CreatedAtAction(nameof(GetWishlistById), new { id = createdWishlist.WishlistId }, createdWishlist);
            }
            catch (DbUpdateException dbEx)
            {
                var innerException = dbEx.InnerException?.Message ?? dbEx.Message;
                return StatusCode(500);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // Delete a wishlist
        [HttpDelete("{id}")]
        public async Task<ActionResult<List<Wishlist>>> DeleteWishlist(Guid id)
        {
            var wishlists = await _wishlistService.DeleteWishlist(id);
            if (wishlists == null)
            {
                return NotFound(new {message = "Wishlist not found. Deletion failed."});
            }
            return Ok(new {message = "Wishlist deleted successfully.", data = wishlists});
        }

        // Add a product to a wishlist
        [HttpPost("{wishlistId}/AddProduct/{productId}")]
        public async Task<ActionResult> AddProductToWishlist(Guid wishlistId, Guid productId)
        {
            var result = await _wishlistService.AddProductToWishlist(wishlistId, productId);
            if (!result)
            {
                return NotFound(new {message = "Failed to add product. Wishlist or Product not found."});
            }
            return Ok(new {message = "Product added to wishlist successfully." });
        }

        // Remove a product from a wishlist
        [HttpDelete("{wishlistId}/RemoveProduct/{productId}")]
        public async Task<IActionResult> RemoveProductFromWishlist(Guid wishlistId, Guid productId)
        {
            var result = await _wishlistService.RemoveProductFromWishlist(wishlistId, productId);
            if (!result)
            {
                return NotFound(new {message = "The product is not assigned to this wishlist."});
            }
            return Ok(new {message = "The product was successfully removed from the wishlist."});
        }
    }
}
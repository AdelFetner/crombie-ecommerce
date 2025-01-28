using crombie_ecommerce.Models.Entities;
using crombie_ecommerce.BusinessLogic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace crombie_ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<Cart>> GetCart(Guid userId)
        {
            var cart = await _cartService.GetOrCreateCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        public async Task<ActionResult<Cart>> AddToCart(Guid userId, Guid productId, int quantity)
        {
            var cart = await _cartService.AddToCartAsync(userId, productId, quantity);
            return Ok(cart);
        }

        [HttpDelete("{userId}/items/{productId}")]
        public async Task<ActionResult<Cart>> RemoveFromCart(Guid userId, Guid productId)
        {
            var cart = await _cartService.RemoveFromCartAsync(userId, productId);
            return Ok(cart);
        }

        [HttpPut("{userId}/items/{productId}")]
        public async Task<ActionResult> UpdateQuantity(Guid userId, Guid productId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(userId, productId, quantity);
            return NoContent();
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult> ClearCart(Guid userId)
        {
            await _cartService.ClearCartAsync(userId);
            return NoContent();
        }
    }

}

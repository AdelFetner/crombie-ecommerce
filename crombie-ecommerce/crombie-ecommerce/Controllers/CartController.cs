using crombie_ecommerce.Models.Entities;
using crombie_ecommerce.BusinessLogic;
using Microsoft.AspNetCore.Mvc;
using crombie_ecommerce.Models.Dto;
using Microsoft.AspNetCore.Authorization;

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

        [HttpPost("{userId}")]
        
        public async Task<ActionResult<Cart>> CreateCart(Guid userId)
        {
            var cart = await _cartService.CreateCartAsync(userId);
            return Ok(cart);
        }

        [HttpGet("{userId}")]
        
        public async Task<ActionResult<Cart>> GetCart(Guid userId)
        {
            var cart = await _cartService.GetCartAsync(userId);
            return Ok(cart);
        }

        [HttpPost("{userId}/items")]
        
        public async Task<ActionResult<Cart>> AddToCart(Guid userId, [FromBody] CartItemDto cartItemDto)
        {
            var cart = await _cartService.AddToCartAsync(userId, cartItemDto);
            return Ok(cart);
        }

        [HttpDelete("{userId}/items/{productId}")]
        
        public async Task<ActionResult<Cart>> RemoveFromCart(Guid userId, Guid productId)
        {
            var cart = await _cartService.RemoveFromCartAsync(userId, productId);
            return Ok(cart);
        }

        [HttpPut("{userId}/items/")]
        
        public async Task<ActionResult> UpdateQuantity(Guid userId, [FromBody] CartItemDto cartItemDto)
        {
            await _cartService.UpdateQuantityAsync(userId,cartItemDto);
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

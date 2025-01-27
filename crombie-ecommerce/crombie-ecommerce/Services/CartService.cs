using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using crombie_ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace crombie_ecommerce.Services
{
    public class CartService
    {
        private readonly ShopContext _shopContext;

        public CartService(ShopContext shopContext)
        {
            _shopContext = shopContext;
        }

        public async Task<Cart> GetOrCreateCartAsync(Guid userId)
        {
            var cart = await _shopContext.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                   
                };
                _shopContext.Carts.Add(cart);
                await _shopContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> AddToCartAsync(Guid userId, Guid productId, int quantity)
        {

            var cart = await GetOrCreateCartAsync(userId);
            var product = await _shopContext.Products.FindAsync(productId);

            if (product == null)
                throw new ArgumentException("Product not found");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = productId,
                    Quantity = quantity,
                    Price = product.Price,
                    Total = product.Price * quantity
                };
                cart.Items.Add(cartItem);

            }
            else
            {
                cartItem.Quantity += quantity;
                cartItem.Price = product.Price; 
            }

            await _shopContext.SaveChangesAsync();
            return cart;
        }

        

        public async Task<Cart> RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                
                await _shopContext.SaveChangesAsync();
            }
            return cart;
        }

        public async Task UpdateQuantityAsync(Guid userId, Guid productId, int quantity)
        {
            var cart = await GetOrCreateCartAsync(userId);
            var product = await _shopContext.Products.FindAsync(productId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null && quantity > 0)
            {
                cartItem.Quantity = quantity;
                cartItem.Total = product.Price * quantity;
                await _shopContext.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(Guid userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            cart.Items.Clear();
            
            await _shopContext.SaveChangesAsync();
        }
    }

}

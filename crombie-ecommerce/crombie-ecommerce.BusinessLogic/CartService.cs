using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace crombie_ecommerce.BusinessLogic
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
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                _shopContext.Carts.Add(cart);
                await _shopContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> AddToCartAsync(Guid userId, Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cart = await GetOrCreateCartAsync(userId);
            var product = await _shopContext.Products.FindAsync(productId);

            if (product == null)
                throw new ArgumentException("Product not found");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
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
                cartItem.Total = cartItem.Price * cartItem.Quantity;
            }

            cart.TotalAmount = cart.Items.Sum(item => item.Total);
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
                cart.TotalAmount = cart.Items.Sum(item => item.Total);
                await _shopContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> UpdateQuantityAsync(Guid userId, Guid productId, int quantity)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cart = await GetOrCreateCartAsync(userId);
            var product = await _shopContext.Products.FindAsync(productId);

            if (product == null)
                throw new ArgumentException("Product not found");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                cartItem.Price = product.Price;
                cartItem.Total = cartItem.Price * cartItem.Quantity;

                cart.TotalAmount = cart.Items.Sum(item => item.Total);
                await _shopContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> ClearCartAsync(Guid userId)
        {
            var cart = await GetOrCreateCartAsync(userId);
            cart.Items.Clear();
            cart.TotalAmount = 0;

            await _shopContext.SaveChangesAsync();

            return cart;
        }
    }

}

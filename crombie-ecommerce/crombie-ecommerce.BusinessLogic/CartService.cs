using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Microsoft.AspNetCore.Mvc;
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


        //Create a cart
        public async Task<Cart> CreateCartAsync(Guid userId)
        {
            
            var existingCart = await _shopContext.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingCart != null)
                throw new InvalidOperationException("This User already has an active cart");

            var cart = new Cart
            {
                UserId = userId,
                Items = new List<CartItem>()
            };

            _shopContext.Carts.Add(cart);
            await _shopContext.SaveChangesAsync();

            return cart;
        }

        //Get a cart (already exist):
        public async Task<Cart> GetCartAsync(Guid userId)
        {
            var cart = await _shopContext.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                throw new Exception("Cart not found for that user.");

            return cart;
        }

        public async Task<Cart> AddToCartAsync(Guid userId, CartItemDto cartItemDto)
        {
            if (cartItemDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cart = await GetCartAsync(userId);
            var product = await _shopContext.Products.FindAsync(cartItemDto.ProductId);

            if (product == null)
                throw new ArgumentException("Product not found");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDto.ProductId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartId = cart.CartId,
                    ProductId = cartItemDto.ProductId,
                    Quantity = cartItemDto.Quantity,
                    Price = product.Price,
                    Total = product.Price * cartItemDto.Quantity,
                };
                cart.Items.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += cartItemDto.Quantity;
                cartItem.Price = product.Price;
                cartItem.Total = cartItem.Price * cartItem.Quantity;
            }

            cart.TotalAmount = cart.Items.Sum(item => item.Total);
            await _shopContext.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> RemoveFromCartAsync(Guid userId, Guid productId)
        {
            var cart = await GetCartAsync(userId);
            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == productId);

            if (cartItem != null)
            {
                cart.Items.Remove(cartItem);
                cart.TotalAmount = cart.Items.Sum(item => item.Total);
                await _shopContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> UpdateQuantityAsync(Guid userId, CartItemDto cartItemDto)
        {
            if (cartItemDto.Quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero");

            var cart = await GetCartAsync(userId);
            var product = await _shopContext.Products.FindAsync(cartItemDto.ProductId);

            if (product == null)
                throw new ArgumentException("Product not found");

            var cartItem = cart.Items.FirstOrDefault(i => i.ProductId == cartItemDto.ProductId);

            if (cartItem != null)
            {
                cartItem.Quantity = cartItemDto.Quantity;
                cartItem.Price = product.Price;
                cartItem.Total = cartItem.Price * cartItem.Quantity;

                cart.TotalAmount = cart.Items.Sum(item => item.Total);
                await _shopContext.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> ClearCartAsync(Guid userId)
        {
            var cart = await GetCartAsync(userId);
            cart.Items.Clear();
            cart.TotalAmount = 0;

            await _shopContext.SaveChangesAsync();

            return cart;
        }
    }

}

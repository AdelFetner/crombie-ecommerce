using crombie_ecommerce.Models;
using crombie_ecommerce.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace crombie_ecommerce.Services
{
    public class WishlistService
    {
        private readonly ShopContext _context;

        public WishlistService(ShopContext context)
        {
            _context = context;
        }

        //Method to get all wishlist
        public async Task<List<Wishlist>> GetAllWishlists()
        {
            return await _context.Wishlists.ToListAsync();
        }

        //Second method to get all wishlist (ask if this is necessary)
        //public List<Wishlist> GetAllWishlists()
        //{
        //    return _context.Wishlists.ToList();
        //}

        //Method to get wishlist by ID
        public async Task<Wishlist?> GetWishlistById(Guid wishlistId)
        {
            return await _context.Wishlists.FindAsync(wishlistId);
        }

        //Second method to get wishlist by ID (ask if this is necessary)
        //public List<Wishlist> GetWishlistByUserId(Guid userId)
        //{
        //    return _context.Wishlists.Where(w => w.UserId == userId).ToList();
        //}

        //Third method to get wishlist by id (ask if this is necessary)
        //public Wishlist? GetWishlistByUserId(Guid userId)
        //{
        //    return _context.Wishlists.SingleOrDefault(w => w.UserId == userId);
        //}

        //Method to create a new wishlist
        public async Task<List<Wishlist>> CreateWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return await _context.Wishlists.ToListAsync();
        }

        // Second method to create a new wishlist (ask if this is necessary)
        //public List<Wishlist> CreateWishlist(Wishlist wishlist)
        //{
        //    _context.Wishlists.Add(wishlist);
        //    _context.SaveChanges();
        //    return _context.Wishlists.ToList();
        //}

        //Method to delete a wishlist
        public async Task<List<Wishlist>> DeleteWishlist(Guid wishlistId)
        {
            var wishlist = await _context.Wishlists.FindAsync(wishlistId);
            if (wishlist == null)
            {
                return null;
            }
            _context.Wishlists.Remove(wishlist);
            await _context.SaveChangesAsync();
            return await _context.Wishlists.ToListAsync();
        }

        //Second method to delete a wishlist (ask if this is necessary)
        //public List<Wishlist> DeleteWishlist(Guid wishlistId)
        //{
        //    var wishlist = _context.Wishlists.Find(wishlistId);
        //    if (wishlist == null)
        //    {
        //        return null;

        //(ask if this is necessary)
        //throw new KeyNotFoundException($"Wishlist with ID {wishlistId} not found.");
        //    }

        //    _context.Wishlists.Remove(wishlist);
        //    _context.SaveChanges();

        //    return _context.Wishlists.ToList();
        //}

        //Method to add a product to a wishlist
        public async Task<bool> AddProductToWishlist(Guid wishlistId, Guid productId)
        {
            var wishlist = await _context.Wishlists.FindAsync(wishlistId);
            if (wishlist == null)
            {
                return false;
            }

            product.WishlistId = wishlistId;
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return true;
        }
        //Method to remove a product from a wishlist
        public async Task<bool> RemoveProductFromWishlist(Guid wishlistId, Guid productId)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.WishlistId == wishlistId && p.Id == productId);
            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
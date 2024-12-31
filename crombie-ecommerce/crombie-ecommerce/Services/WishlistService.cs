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
        public List<Wishlist> GetAllWishlists()
        {
            return _context.Wishlists.ToList();
        }

        //Method to get wishlist by ID
        public List<Wishlist> GetWishlistByUserId(Guid userId)
        {
            return _context.Wishlists.Where(w => w.UserId == userId).ToList();
        }

        //Second method to get wishlist by id (ask if this is necessary)
        //public Wishlist? GetWishlistByUserId(Guid userId)
        //{
        //    return _context.Wishlists.SingleOrDefault(w => w.UserId == userId);
        //}

        //Method to create a new wishlist
        public List<Wishlist> CreateWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();
            return _context.Wishlists.ToList();
        }

        //Method to delete a wishlist
        public List<Wishlist> DeleteWishlist(Guid wishlistId)
        {
            var wishlist = _context.Wishlists.Find(wishlistId);
            if (wishlist == null)
            {
                return null;

                //(ask if this is necessary)
                //throw new KeyNotFoundException($"Wishlist with ID {wishlistId} not found.");
            }

            _context.Wishlists.Remove(wishlist);
            _context.SaveChanges();

            return _context.Wishlists.ToList();
        }
    }
}
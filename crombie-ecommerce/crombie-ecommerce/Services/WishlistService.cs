using crombie_ecommerce.Contexts;
using Microsoft.EntityFrameworkCore;
using crombie_ecommerce.Models.Models.Entities;

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

        //Method to get wishlist by ID
        public async Task<Wishlist?> GetWishlistById(Guid wishlistId)
        {
            return await _context.Wishlists.FindAsync(wishlistId);
        }

        //Method to create a new wishlist
        public async Task<List<Wishlist>> CreateWishlist(Wishlist wishlist)
        {
            var userExists = await _context.Users.AnyAsync(u => u.UserId == wishlist.UserId);
            if (!userExists)
            {
                throw new Exception("User not found");
            }
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
            return await _context.Wishlists.ToListAsync();
        }

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
        //Method to add a product to a wishlist
        public async Task<bool> AddProductToWishlist(Guid wishlistId, Guid productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.WishlistId == wishlistId);

            if (wishlist == null)
            {
                throw new Exception("Wishlist not found");
            }

            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            wishlist.Products.Add(product);
            await _context.SaveChangesAsync();

            return true;
        }

        //Method to remove a product from a wishlist
        public async Task<bool> RemoveProductFromWishlist(Guid wishlistId, Guid productId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.Products)
                .FirstOrDefaultAsync(w => w.WishlistId == wishlistId);

            if (wishlist == null)
            {
                throw new Exception("Wishlist not found");
            }
            
            var productToRemove = wishlist.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (productToRemove == null)
            {
                throw new Exception("Product not found in the wishlist");
            }

            wishlist.Products.Remove(productToRemove);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
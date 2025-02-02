using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class TagService
    {
        private readonly ShopContext _context;

        public TagService(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.ToListAsync();
        }

        public async Task<Tag?> GetTagById(Guid tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        public async Task<Tag> CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return tag;
        }

        public async Task<List<Tag>> DeleteTag(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null) return await _context.Tags.ToListAsync();

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return await _context.Tags.ToListAsync();
        }

        public async Task<bool> AddTagToWishlist(Guid tagId, Guid wishlistId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            var wishlist = await _context.Wishlists
                .Include(w => w.Tags)
                .FirstOrDefaultAsync(w => w.WishlistId == wishlistId);

            if (tag == null || wishlist == null) return false;

            // Add tag to wishlist's collection
            wishlist.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveTagFromWishlist(Guid tagId)
        {
            var tag = await _context.Tags
                .Include(t => t.Wishlist)
                .FirstOrDefaultAsync(t => t.TagId == tagId);

            if (tag?.Wishlist == null) return false;

            // Remove tag from wishlist
            tag.Wishlist.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
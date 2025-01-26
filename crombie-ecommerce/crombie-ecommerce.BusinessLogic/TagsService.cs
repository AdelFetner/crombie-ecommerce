﻿using crombie_ecommerce.DataAccess.Contexts;
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

        // Method to get all tags
        public async Task<List<Tag>> GetAllTags()
        {
            return await _context.Tags.ToListAsync();
        }

        // Method to get a tag by its ID
        public async Task<Tag?> GetTagById(Guid tagId)
        {
            return await _context.Tags.FindAsync(tagId);
        }

        // Method to create a new tag
        public async Task<List<Tag>> CreateTag(Tag tag)
        {
            _context.Tags.Add(tag);
            await _context.SaveChangesAsync();
            return await _context.Tags.ToListAsync();
        }

        // Method to delete a tag
        public async Task<List<Tag>> DeleteTag(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null)
            {
                return null;
            }

            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();
            return await _context.Tags.ToListAsync();
        }

        // Method to associate a tag with a wishlist
        public async Task<bool> AddTagToWishlist(Guid tagId, Guid wishlistId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null)
            {
                return false;
            }

            var wishlist = await _context.Wishlists.FindAsync(wishlistId);
            if (wishlist == null)
            {
                return false;
            }

            tag.WishlistId = wishlistId;
            wishlist.TagsId = tagId;
            await _context.SaveChangesAsync();
            return true;
        }

        // Method to dissociate a tag from a wishlist
        public async Task<bool> RemoveTagFromWishlist(Guid tagId)
        {
            var tag = await _context.Tags.FindAsync(tagId);
            if (tag == null || tag.WishlistId == null)
            {
                return false;
            }

            tag.WishlistId = null;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}


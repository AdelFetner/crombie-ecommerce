using crombie_ecommerce.Contexts;
using crombie_ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.Services
{
    public class TagsService
    {
            private readonly ShopContext _context;

            public TagsService(ShopContext context)
            {
                _context = context;
            }

            // Method to get all tags
            public async Task<List<Tags>> GetAllTags()
            {
                return await _context.Tags.ToListAsync();
            }

            // Method to get a tag by its ID
            public async Task<Tags?> GetTagById(Guid tagId)
            {
                return await _context.Tags.FindAsync(tagId);
            }

            // Method to create a new tag
            public async Task<List<Tags>> CreateTag(Tags tag)
            {
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();
                return await _context.Tags.ToListAsync();
            }

            // Method to delete a tag
            public async Task<List<Tags>> DeleteTag(Guid tagId)
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
        }
    }


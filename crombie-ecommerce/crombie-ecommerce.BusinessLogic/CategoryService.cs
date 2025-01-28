﻿using crombie_ecommerce.DataAccess.Contexts;
using crombie_ecommerce.Models.Dto;
using crombie_ecommerce.Models.Entities;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace crombie_ecommerce.BusinessLogic
{
    public class CategoryService : ICategoryService
    {
        private readonly ShopContext _context;

        public CategoryService(ShopContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<Category> CreateCategory(CategoryDto categoryDto)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = categoryDto.Name,
                Description = categoryDto.Description
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<Category> UpdateCategory(Guid id, Category updatedCategory)
        {
            // look up for the first category with the given id by reusing previous get by id method
            var existingCategory = await GetCategoryById(id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }

            // prop update
            existingCategory.Name = updatedCategory.Name;
            existingCategory.Description = updatedCategory.Description;

            _context.Categories.Update(existingCategory);
            await _context.SaveChangesAsync();

            return existingCategory;

        }

        public async Task DeleteCategory(Guid id)
        {
            var existingCategory = await GetCategoryById(id);
            if (existingCategory == null)
            {
                throw new Exception("Category not found");
            }

            _context.Categories.Remove(existingCategory);
            await _context.SaveChangesAsync();
        }
    }
}
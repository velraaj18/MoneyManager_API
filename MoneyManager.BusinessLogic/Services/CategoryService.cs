using System;
using MoneyManager.Data;
using MoneyManager.Services;
using MoneyManager.Models;
using Microsoft.EntityFrameworkCore;
using MoneyManager.DTO;

namespace MoneyManager.Services
{
    public class CategoryService
    {
        public readonly AppDBContext _db;

        public CategoryService(AppDBContext db)
        {
            _db = db;
        }

        public async Task<APIResponse<List<Category>>> GetAllCategories()
        {
            var result = await _db.Categories.ToListAsync();

            if (result == null)
                return new APIResponse<List<Category>> { Data = null, Message = "No Categories found", StatusCode = 200 };

            return new APIResponse<List<Category>> { Data = result, Message = "Categories fetched successfully", StatusCode = 200 };
        }

        public async Task<APIResponse<Category>> PostCategory(CategoryRequest req)
        {
            if (req == null)
                return new APIResponse<Category> { StatusCode = 400, Message = "You must provide the details", Data = null };

            var category = new Category()
            {
                CategoryName = req.CategoryName,
                Description = req.Description
            };

            _db.Categories.Add(category);
            await _db.SaveChangesAsync();

            return new APIResponse<Category>
            {
                StatusCode = 201,
                Message = "Category added successfully",
                Data = category
            };
        }

        public async Task<APIResponse<Category>> UpdateCategory(int id, CategoryRequest req)
        {
            if (req == null)
                return new APIResponse<Category> { StatusCode = 400, Message = "You must provide the details", Data = null };

            var category = await _db.Categories.FindAsync(id);

            if (category == null)
                return new APIResponse<Category> { StatusCode = 404, Message = "Category not found", Data = null };

            category.CategoryName = req.CategoryName;
            category.Description = req.Description;

            await _db.SaveChangesAsync();

            return new APIResponse<Category>
            {
                StatusCode = 200,
                Message = "Category updated successfully",
                Data = category
            };
        }

        public async Task<APIResponse<Category>> DeleteCategory(int id)
        {
            var category = await _db.Categories.FindAsync(id);

            if (category == null)
                return new APIResponse<Category> { StatusCode = 404, Message = "Category not found", Data = null };

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return new APIResponse<Category>
            {
                StatusCode = 200,
                Message = "Category deleted successfully",
                Data = category
            };
        }
    }
}
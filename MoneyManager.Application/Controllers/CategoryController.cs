using MoneyManager.DTO;
using MoneyManager.Models;
using MoneyManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace MoneyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        public readonly CategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpGet("GetAllCategories")]
        [Authorize]
        public Task<APIResponse<List<Category>>> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            
            var response = _service.GetAllCategories(userId);
            return response;
        }

        [HttpPost]
        [Authorize]
        public Task<APIResponse<Category>> Post(CategoryRequest req)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            req.UserId = userId;

            var response = _service.PostCategory(req);
            return response;
        }

        [HttpPut("{id}")]
        [Authorize]
        public Task<APIResponse<Category>> Update(int id, CategoryRequest req)
        {
            var response = _service.UpdateCategory(id, req);
            return response;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public Task<APIResponse<Category>> Delete(int id)
        {
            var response = _service.DeleteCategory(id);
            return response;
        }
    }
}
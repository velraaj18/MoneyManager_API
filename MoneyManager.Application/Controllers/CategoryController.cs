using MoneyManager.DTO;
using MoneyManager.Models;
using MoneyManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public Task<APIResponse<List<Category>>> GetAll()
        {
            var response = _service.GetAllCategories();
            return response;
        }

        [HttpPost]
        public Task<APIResponse<Category>> Post(CategoryRequest req)
        {
            var response = _service.PostCategory(req);
            return response;
        }

        [HttpPut("{id}")]
        public Task<APIResponse<Category>> Update(int id, CategoryRequest req)
        {
            var response = _service.UpdateCategory(id, req);
            return response;
        }

        [HttpDelete("{id}")]
        public Task<APIResponse<Category>> Delete(int id)
        {
            var response = _service.DeleteCategory(id);
            return response;
        }
    }
}
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.BusinessLogic.Services;
using MoneyManager.Common.DTOs.Budgets;
using MoneyManager.DTO;
using MoneyManager.Models;

namespace MoneyManager.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetService _service;
        public BudgetController(BudgetService service)
        {
            _service = service;
        }

        [HttpGet]
        public Task<APIResponse<List<BudgetResponse>>> GetAllBudget(int userId)
        {
            var response = _service.GetAllBudget(userId);
            return response;
        }

        [HttpGet]
        public Task<APIResponse<List<BudgetResponse>>> GetBudgetByCategory(int userId, int? categoryId, int? month, int? year)
        {
            var response = _service.GetBudgetByCategory(userId: userId, categoryId: categoryId, month: month, year: year);
            return response;
        }

        [HttpPost("AddBudget")]
        public Task<APIResponse<Budget>> AddBudget(BudgetRequest request)
        {
            var response = _service.CreateBudget(request);
            return response;
        }

        [HttpPut("UpdateBudget")]
        public Task<APIResponse<Budget>> Update(BudgetRequest request)
        {
            var response = _service.UpdateBudget(request);
            return response;
        }
    }
}

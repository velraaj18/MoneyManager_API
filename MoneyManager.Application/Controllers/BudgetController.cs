using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("GetAllBudget")]
        [Authorize]
        public Task<APIResponse<List<BudgetResponse>>> GetAllBudget()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = _service.GetAllBudget(userId);
            return response;
        }

        [HttpGet("GetBudgetByCategory")]
        [Authorize]
        public Task<APIResponse<List<BudgetResponse>>> GetBudgetByCategory(int? categoryId, int? month, int? year)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = _service.GetBudgetByCategory(userId: userId, categoryId: categoryId, month: month, year: year);
            return response;
        }

        [HttpPost("AddBudget")]
        [Authorize]
        public Task<APIResponse<Budget>> AddBudget(BudgetRequest request)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            request.UserId = userId;
            
            var response = _service.CreateBudget(request);
            return response;
        }

        [HttpPut("UpdateBudget")]
        [Authorize]
        public Task<APIResponse<Budget>> Update(BudgetRequest request)
        {
            var response = _service.UpdateBudget(request);
            return response;
        }
    }
}

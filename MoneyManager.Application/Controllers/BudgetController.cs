using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.BusinessLogic.Services;
using MoneyManager.Common.DTOs.Budgets;
using MoneyManager.DTO;

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
        public Task<APIResponse<List<BudgetResponse>>> GetAllBudget()
        {
            var response = _service.GetAllBudget();
            return response;
        }
    }
}

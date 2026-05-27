using MoneyManager.DTO;
using MoneyManager.DTO.Transactions;
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
    public class TransactionsController : ControllerBase
    {
        public readonly TransactionService _service;

        public TransactionsController(TransactionService service)
        {
            _service = service;
        }

        [HttpGet("GetAllTransactions")]
        [Authorize]
        public Task<APIResponse<List<TransactionResponse>>> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);          
            var response = _service.GetAllTransactions(userId);
            return response;
        }

        [HttpPost]
        [Authorize]
        public Task<APIResponse<Transaction>> Post(TransactionRequest req)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            req.UserId = userId;

            var response = _service.PostTransaction(req);
            return response;
        }

        [HttpPut("{id}")]
        [Authorize]
        public Task<APIResponse<Transaction>> Update(int id, TransactionRequest req)
        {
            var response = _service.UpdateTransaction(id, req);
            return response;
        }

        [HttpDelete("{id}")]
        public Task<APIResponse<Transaction>> Delete(int id)
        {
            var response = _service.DeleteTransaction(id);
            return response;
        }

        [HttpGet("Category-Summary")]
        public Task<APIResponse<List<TransactionCategorySummary>>> GetByCategory(DateOnly? startDate, DateOnly? endDate)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            return _service.GetByCategory(userId, startDate, endDate);
        }

        [HttpGet("Account-Summary")]
        public Task<APIResponse<List<TransactionAccountSummary>>> GetByAccount(DateOnly? startDate, DateOnly? endDate)
        {
            return _service.GetByAccount(startDate, endDate);
        }

        [HttpGet("Month-Summary")]
        public Task<APIResponse<List<TransactionMonthSummary>>> GetByMonth(DateOnly? startDate, DateOnly? endDate)
        {
            return _service.GetByMonth(startDate, endDate);
        }
    }
}

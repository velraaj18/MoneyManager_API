using MoneyManager.DTO;
using MoneyManager.DTO.Transactions;
using MoneyManager.Models;
using MoneyManager.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public Task<APIResponse<List<TransactionResponse>>> GetAll()
        {
            var response = _service.GetAllTransactions();
            return response;
        }

        [HttpPost]
        public Task<APIResponse<Transaction>> Post(TransactionRequest req)
        {
            var response = _service.PostTransaction(req);
            return response;
        }

        [HttpPut("{id}")]
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
        public Task<APIResponse<List<TransactionCategorySummary>>> GetByCategory(DateTime? startDate, DateTime? endDate)
        {
            return _service.GetByCategory(startDate, endDate);
        }

        [HttpGet("Account-Summary")]
        public Task<APIResponse<List<TransactionAccountSummary>>> GetByAccount(DateTime? startDate, DateTime? endDate)
        {
            return _service.GetByAccount(startDate, endDate);
        }

        [HttpGet("Month-Summary")]
        public Task<APIResponse<List<TransactionMonthSummary>>> GetByMonth(DateTime? startDate, DateTime? endDate)
        {
            return _service.GetByMonth(startDate, endDate);
        }
    }
}

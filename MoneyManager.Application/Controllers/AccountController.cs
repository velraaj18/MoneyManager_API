using MoneyManager.DTO;
using MoneyManager.Models;
using MoneyManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace MoneyManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        public readonly AccountService _service;

        public AccountsController(AccountService service)
        {
            _service = service;
        }

        [HttpGet("GetAllAccounts")]
        public Task<APIResponse<List<Account>>> GetAll()
        {
            var response = _service.GetAllAccounts();
            return response;
        }

        [HttpPost]
        public Task<APIResponse<Account>> Post(AccountRequest req)
        {
            var response = _service.PostAccount(req);
            return response;
        }

        [HttpPut("{id}")]
        public Task<APIResponse<Account>> Update(int id, AccountRequest req)
        {
            var response = _service.UpdateAccount(id, req);
            return response;
        }

        [HttpDelete("{id}")]
        public Task<APIResponse<Account>> Delete(int id)
        {
            var response = _service.DeleteAccount(id);
            return response;
        }
    }
}
using MoneyManager.DTO;
using MoneyManager.Models;
using MoneyManager.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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
        [Authorize]
        public Task<APIResponse<List<Account>>> GetAll()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var response = _service.GetAllAccounts(userId);
            return response;
        }

        [HttpPost]
        [Authorize]
        public Task<APIResponse<Account>> Post(AccountRequest req)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            req.UserId = userId;

            var response = _service.PostAccount(req);
            return response;
        }

        [HttpPut("{id}")]
        [Authorize]
        public Task<APIResponse<Account>> Update(int id, AccountRequest req)
        {
            var response = _service.UpdateAccount(id, req);
            return response;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public Task<APIResponse<Account>> Delete(int id)
        {
            var response = _service.DeleteAccount(id);
            return response;
        }
    }
}
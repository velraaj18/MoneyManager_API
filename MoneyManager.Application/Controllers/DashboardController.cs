using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.BusinessLogic.Services;
using MoneyManager.Common.DTOs.Dashboard;
using MoneyManager.DTO;

namespace MoneyManager.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly DashboardService _service;
        public DashboardController(DashboardService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet("summary")]
        public async Task<APIResponse<DashboardSummary>> GetSummary()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _service.GetDashboardSummary(userId);

            return response;
        }
        
    }
}

using Microsoft.EntityFrameworkCore;
using MoneyManager.Common.DTOs.Dashboard;
using MoneyManager.Data;
using MoneyManager.DTO;

namespace MoneyManager.BusinessLogic.Services;

public class DashboardService
{
    private readonly AppDBContext _db;
    private readonly BudgetService _service;
    public DashboardService(AppDBContext db, BudgetService service)
    {
        _db = db;
        _service = service;
    }

    public async Task<APIResponse<DashboardSummary>> GetDashboardSummary(string userId)
    {
        var validateUserId = int.TryParse(userId, out int result);
        if (validateUserId)
        {
            var totalIncome = GetTotalIncome(result);
            var totalExpense = GetTotalExpense(result);
            var balance = totalIncome - totalExpense;

            var topSpentCategory = GetTopSpentCategory(result);
            var budgetExceeded = 0;

            var budgetResponse = _service.GetAllBudget(result).Result.Data;

            foreach(var item in budgetResponse)
            {
                if(item.AmountRemaining < 0)
                {
                    budgetExceeded++;
                }
            }

            var response = new DashboardSummary()
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance,
                TopSpendingCategory = topSpentCategory,
                BudgetExceeded = budgetExceeded
            };

            return new APIResponse<DashboardSummary> {Data = response, Message = "Dashboard Summary retrieved", StatusCode=200};
        }
        
        return new APIResponse<DashboardSummary>{Data = null, Message = "User Id not found", StatusCode = 404};
    }

    private decimal GetTotalIncome(int userId)
    {
        var totalIncome = _db.Transactions.AsNoTracking().Where(x=> x.UserId == userId && x.Category.TransactionType == Common.Enums.TransactionTypeCode.Income).Sum(x => x.Amount);

        return totalIncome;
    }

    private decimal GetTotalExpense(int userId)
    {
        var totalExpense = _db.Transactions.AsNoTracking().Where(x=> x.UserId == userId && x.Category.TransactionType == Common.Enums.TransactionTypeCode.Expense).Sum(x => x.Amount);

        return totalExpense;
    }

    private string GetTopSpentCategory(int userId)
    {
        var topSpentCategory = _db.Transactions.AsNoTracking().Where(x => x.UserId == userId && x.Category.TransactionType == Common.Enums.TransactionTypeCode.Expense).GroupBy(x=> x.Category.CategoryName).Select(g=> new
        {
            CategoryName = g.Key,
            TotalAmount = g.Sum(x => x.Amount)
        }).OrderByDescending(x => x.TotalAmount).Select(x=> x.CategoryName).FirstOrDefault();

        return topSpentCategory;
    }
}

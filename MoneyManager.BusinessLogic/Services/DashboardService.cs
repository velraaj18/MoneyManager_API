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
        var isValidUserId = int.TryParse(userId, out int parsedUserId);
        if (isValidUserId)
        {
            var incomeTask = GetTotalIncome(parsedUserId);
            var expenseTask = GetTotalExpense(parsedUserId);

            var topSpentCategoryTask = GetTopSpentCategory(parsedUserId);
            var budgetTask =  _service.GetAllBudget(parsedUserId);

            await Task.WhenAll(incomeTask, expenseTask, topSpentCategoryTask, budgetTask);

            var totalIncome = incomeTask.Result;
            var totalExpense = expenseTask.Result;
            var topSpentCategory = topSpentCategoryTask.Result;
            var budgetResponse = budgetTask.Result;

            var balance = totalIncome - totalExpense;
            var budgetExceeded = budgetResponse.Data.Count(x => x.AmountRemaining < 0);

            var response = new DashboardSummary()
            {
                TotalIncome = totalIncome,
                TotalExpense = totalExpense,
                Balance = balance,
                TopSpendingCategory = topSpentCategory ?? "No Expenses",
                BudgetExceeded = budgetExceeded
            };

            return new APIResponse<DashboardSummary> {Data = response, Message = "Dashboard Summary retrieved", StatusCode=200};
        }
        
        return new APIResponse<DashboardSummary>{Data = null, Message = "User Id not found", StatusCode = 404};
    }

    private async Task<decimal> GetTotalIncome(int userId)
    {
        var totalIncome = await _db.Transactions.AsNoTracking().Where(x=> x.UserId == userId && x.Category.TransactionType == Common.Enums.TransactionTypeCode.Income).SumAsync(x => x.Amount);

        return totalIncome;
    }

    private async Task<decimal> GetTotalExpense(int userId)
    {
        var totalExpense = await _db.Transactions.AsNoTracking().Where(x=> x.UserId == userId && x.Category.TransactionType == Common.Enums.TransactionTypeCode.Expense).SumAsync(x => x.Amount);

        return totalExpense;
    }

    private async Task<string> GetTopSpentCategory(int userId)
    {
        var topSpentCategory = _db.Transactions.AsNoTracking().Where(x => x.UserId == userId && x.Category.TransactionType == Common.Enums.TransactionTypeCode.Expense).GroupBy(x=> x.Category.CategoryName).Select(g=> new
        {
            CategoryName = g.Key,
            TotalAmount = g.Sum(x => x.Amount)
        }).OrderByDescending(x => x.TotalAmount).Select(x=> x.CategoryName).FirstOrDefault();

        return topSpentCategory;
    }
}

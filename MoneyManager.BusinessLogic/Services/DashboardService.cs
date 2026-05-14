using Microsoft.EntityFrameworkCore;
using MoneyManager.Common.DTOs.Dashboard;
using MoneyManager.Data;
using MoneyManager.DTO;
using MoneyManager.Services;
using MoneyManager.Common.Enums;

namespace MoneyManager.BusinessLogic.Services;

public class DashboardService
{
    private readonly AppDBContext _db;
    private readonly BudgetService _budgetService;
    private readonly TransactionService _transacationService;
    public DashboardService(AppDBContext db, BudgetService budgetService, TransactionService transactionService)
    {
        _db = db;
        _budgetService = budgetService;
        _transacationService = transactionService;
    }

    public async Task<APIResponse<DashboardSummary>> GetDashboardSummary(string userId)
    {
        var isValidUserId = int.TryParse(userId, out int parsedUserId);
        if (isValidUserId)
        {
            var transactionSummary = (await _transacationService.GetByCategory(null, null)).Data;
            var budgetResponse = (await _budgetService.GetAllBudget(parsedUserId)).Data;

            var totalIncome = transactionSummary.Where(x => x.TransactionTypeCode == Common.Enums.TransactionTypeCode.Income).Select(x=> x.TotalAmount).FirstOrDefault();
            var totalExpense = transactionSummary.Where(x => x.TransactionTypeCode == Common.Enums.TransactionTypeCode.Expense).Select(x=> x.TotalAmount).FirstOrDefault();

            var topSpentCategory = transactionSummary.Where(x=> x.TransactionTypeCode == TransactionTypeCode.Expense).OrderByDescending(x => x.TotalAmount).Select(x=> x.CategoryName).FirstOrDefault();
            
            var balance = totalIncome - totalExpense;
            var budgetExceeded = budgetResponse.Count(x => x.AmountRemaining < 0);

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
        var totalIncome = await _db.Transactions.AsNoTracking().Where(x=> x.UserId == userId && x.Category.TransactionType == TransactionTypeCode.Income).SumAsync(x => x.Amount);

        return totalIncome;
    }

    private async Task<decimal> GetTotalExpense(int userId)
    {
        var totalExpense = await _db.Transactions.AsNoTracking().Where(x=> x.UserId == userId && x.Category.TransactionType == TransactionTypeCode.Expense).SumAsync(x => x.Amount);

        return totalExpense;
    }

    private async Task<string> GetTopSpentCategory(int userId)
    {
        var topSpentCategory = await _db.Transactions.AsNoTracking().Where(x => x.UserId == userId && x.Category.TransactionType == TransactionTypeCode.Expense).GroupBy(x=> x.Category.CategoryName).Select(g=> new
        {
            CategoryName = g.Key,
            TotalAmount = g.Sum(x => x.Amount)
        }).OrderByDescending(x => x.TotalAmount).Select(x=> x.CategoryName).FirstOrDefaultAsync();

        return topSpentCategory;
    }
}

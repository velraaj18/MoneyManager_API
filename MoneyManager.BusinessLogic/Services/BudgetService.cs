using System;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Common.DTOs.Budgets;
using MoneyManager.Common.Enums;
using MoneyManager.Data;
using MoneyManager.DTO;
using MoneyManager.Models;

namespace MoneyManager.BusinessLogic.Services;

public class BudgetService
{
    private readonly AppDBContext _db;
    public BudgetService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<APIResponse<List<BudgetResponse>>> GetAllBudget(int userId)
    {
        var response = await _db.Budgets.Include(x => x.Category).Where(x => x.UserId == userId).ToListAsync();
        if (response.Count == 0)
        {
            return new APIResponse<List<BudgetResponse>> { Data = new List<BudgetResponse>(), Message = "No Budgets Found", StatusCode = 200 };
        }
        var budgetResponses = new List<BudgetResponse>();
        var transactions = await _db.Transactions.Where(x => x.UserId == userId && x.Category.TransactionType == TransactionTypeCode.Expense).ToListAsync();

        foreach (var item in response)
        {
            var totalSpent = transactions
                .Where(x =>
                    x.CategoryId == item.CategoryId &&
                    x.Date.Month == item.Month &&
                    x.Date.Year == item.Year)
                .Sum(x => x.Amount);

            budgetResponses.Add(new BudgetResponse()
            {
                BudgetUID = item.BudgetUID,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.CategoryName,
                SpendLimit = item.SpendLimit,
                AmountRemaining = item.SpendLimit - totalSpent,
                Year = item.Year,
                Month = item.Month,
                DateCreated = item.DateCreated,
                ModifiedDate = item.ModifiedDate
            });
        }

        return new APIResponse<List<BudgetResponse>> { Data = budgetResponses, Message = "Budgets fetched successfully", StatusCode = 200 };
    }

    public async Task<APIResponse<List<BudgetResponse>>> GetBudgetByCategory(int userId, int? categoryId, int? month, int? year)
    {
        var query = _db.Budgets.Where(x => x.UserId == userId);

        if (categoryId.HasValue)
        {
            query = query.Where(x => x.CategoryId == categoryId);
        }
        if (month.HasValue)
        {
            query = query.Where(x => x.Month == month);
        }
        if (year.HasValue)
        {
            query = query.Where(x => x.Year == year);
        }

        var budgets = await query.Include(x => x.Category).ToListAsync();
        var transactions = await _db.Transactions.Where(x => x.UserId == userId && x.Category.TransactionType == TransactionTypeCode.Expense).ToListAsync();

        var response = new List<BudgetResponse>();

        foreach (var item in budgets)
        {
            var totalSpent = transactions
                .Where(x =>
                    x.CategoryId == item.CategoryId &&
                    x.Date.Month == item.Month &&
                    x.Date.Year == item.Year)
                .Sum(x => x.Amount);

            response.Add(new BudgetResponse()
            {
                BudgetUID = item.BudgetUID,
                CategoryId = item.CategoryId,
                CategoryName = item.Category?.CategoryName,
                SpendLimit = item.SpendLimit,
                AmountRemaining = item.SpendLimit - totalSpent,
                Year = item.Year,
                Month = item.Month,
                DateCreated = item.DateCreated,
                ModifiedDate = item.ModifiedDate
            });
        }

        if (response.Count == 0)
        {
            return new APIResponse<List<BudgetResponse>> { Data = new List<BudgetResponse>(), Message = "No Budgets Found", StatusCode = 200 };
        }

        return new APIResponse<List<BudgetResponse>> { Data = response, Message = "Budgets fetched successfully", StatusCode = 200 };
    }

    public async Task<APIResponse<Budget>> CreateBudget(BudgetRequest request)
    {
        if (request == null)
        {
            return new APIResponse<Budget> { Data = new Budget(), Message = "Request Cannot be Empty", StatusCode = 400 };
        }

        var duplicateBudget = await _db.Budgets.AnyAsync(x => x.UserId == request.UserId && x.CategoryId == request.CategoryId && x.Month == request.Month && x.Year == request.Year);

        if (duplicateBudget)
            return new APIResponse<Budget> { Data = null, Message = "Budget is already added for this category and month and year", StatusCode = 409 };

        var budget = new Budget()
        {
            UserId = request.UserId,
            CategoryId = request.CategoryId,
            SpendLimit = request.SpendLimit,
            Year = request.Year,
            Month = request.Month,
            DateCreated = DateTime.UtcNow
        };

        _db.Budgets.Add(budget);
        await _db.SaveChangesAsync();

        return new APIResponse<Budget> { Data = budget, Message = "Budget Created Successfully", StatusCode = 201 };
    }

    public async Task<APIResponse<Budget>> UpdateBudget(BudgetRequest request)
    {
        if (request == null || request.BudgetId == null || request.BudgetId == 0)
        {
            return new APIResponse<Budget> { Data = new Budget(), Message = "Request/Budget Id Cannot be Empty", StatusCode = 400 };
        }

        var existingBudget = await _db.Budgets.Where(x => x.BudgetUID == request.BudgetId).FirstOrDefaultAsync();

        if (existingBudget == null)
        {
            return new APIResponse<Budget> { Data = new Budget(), Message = "Budget not found", StatusCode = 400 };
        }

        existingBudget.CategoryId = request.CategoryId;
        existingBudget.SpendLimit = request.SpendLimit;
        existingBudget.Month = request.Month;
        existingBudget.Year = request.Year;
        existingBudget.ModifiedDate = DateTime.UtcNow;
        await _db.SaveChangesAsync();

        return new APIResponse<Budget> { Data = existingBudget, Message = "Budget updated successfully", StatusCode = 200 };
    }
}

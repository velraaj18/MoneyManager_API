using System;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Common.DTOs.Budgets;
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

    public async Task<APIResponse<List<BudgetResponse>>> GetAllBudget()
    {
        var response = await _db.Budgets.Include(x=> x.Category).Include(x=> x.User).ToListAsync();
        if (response.Count == 0)
        {
            return new APIResponse<List<BudgetResponse>>{ Data = null, Message = "No Budgets Found", StatusCode = 200};
        }
        var budgetResponses = new List<BudgetResponse>();

        foreach (var item in response)
        {
            budgetResponses.Add(new BudgetResponse()
            {
                CategoryName = item.Category?.CategoryName,
                SpendLimit = item.SpendLimit,
                AmountRemaining = 0,
                Year = item.Year,
                Month = item.Month,
                DateCreated = item.DateCreated,
                ModifiedDate = item.ModifiedDate
            });
        }

        return new APIResponse<List<BudgetResponse>>{Data = budgetResponses, Message = "Budgets fetched successfully", StatusCode = 200};
    }

    public async Task<APIResponse<Budget>> CreateBudget(BudgetRequest request)
    {
        if(request == null)
        {
            return new APIResponse<Budget>{Data = null, Message = "Request Cannot be Empty", StatusCode = 400};
        }

        var duplicateBudget = await _db.Budgets.AnyAsync(x =>x.UserId == request.UserId &&x.CategoryId == request.CategoryId &&x.Month == request.Month && x.Year == request.Year);

        if(duplicateBudget)
            return new APIResponse<Budget>{Data = null, Message = "Budget is already added for this category and month and year", StatusCode = 409};

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

        return new APIResponse<Budget>{Data = budget, Message = "Budget Created Successfully", StatusCode = 201};
    }
}

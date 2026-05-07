using System;
using Microsoft.EntityFrameworkCore;
using MoneyManager.Common.DTOs.Budgets;
using MoneyManager.Data;
using MoneyManager.DTO;

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
}

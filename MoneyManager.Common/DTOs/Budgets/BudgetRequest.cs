using System;

namespace MoneyManager.Common.DTOs.Budgets;

public class BudgetRequest
{
    public int? BudgetId {get; set;}
    public int UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal SpendLimit { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
}

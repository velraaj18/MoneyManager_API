using System;

namespace MoneyManager.Common.DTOs.Budgets;

public class BudgetResponse
{
    public string CategoryName {get; set;}
    public string UserName {get; set;}
    public decimal SpendLimit {get; set;}
    public decimal AmountRemaining {get; set;}
    public int Month { get; set; }
    public int Year { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

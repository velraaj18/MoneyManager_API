using System;

namespace MoneyManager.Common.DTOs.Dashboard;

public class DashboardSummary
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal Balance { get; set; }
    public string TopSpendingCategory { get; set; }
    public int BudgetExceeded { get; set; }
}

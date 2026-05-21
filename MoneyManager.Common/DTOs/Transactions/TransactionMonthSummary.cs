using System;
using MoneyManager.Common.Enums;

namespace MoneyManager.DTO.Transactions
{
    public class TransactionMonthSummary
    {
        public int Year { get; set; }
        public int MonthNumber { get; set; }
        public string Month { get; set; } = string.Empty;
        public TransactionTypeCode TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}

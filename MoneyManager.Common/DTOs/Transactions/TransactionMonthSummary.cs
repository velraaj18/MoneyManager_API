using System;

namespace MoneyManager.DTO.Transactions
{
    public class TransactionMonthSummary
    {
        public int Year { get; set; }
        public string Month { get; set; }
        public int TransactionType { get; set; }
        public decimal Amount { get; set; }
    }
}

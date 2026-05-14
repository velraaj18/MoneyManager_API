using System;
using MoneyManager.Common.Enums;

namespace MoneyManager.DTO.Transactions
{
    public class TransactionCategorySummary
    {
        public string CategoryName { get; set; }
        public TransactionTypeCode TransactionTypeCode { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

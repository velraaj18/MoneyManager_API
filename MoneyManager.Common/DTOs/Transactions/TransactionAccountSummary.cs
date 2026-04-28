using System;

namespace MoneyManager.DTO.Transactions
{
    public class TransactionAccountSummary
    {
        public string AccountName {get; set;}
        public string CategoryName {get; set;}
        public decimal TotalAmount {get; set;}

    }
}

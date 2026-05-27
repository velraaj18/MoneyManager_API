using System;

namespace MoneyManager.DTO
{
    public class TransactionRequest
    {
        public int UserId { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }
        public DateOnly Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}

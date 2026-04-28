using System;

namespace MoneyManager.DTO
{
    public class TransactionRequest
    {
        public int CategoryUID { get; set; }
        public int AccountUID { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
    }
}

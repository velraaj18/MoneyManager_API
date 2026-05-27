using System;
using MoneyManager.Common.Enums;

namespace MoneyManager.DTO
{
    public class CategoryRequest
    {
        public string CategoryName { get; set; }
        public int UserId { get; set; }
        public TransactionTypeCode TransactionType { get; set; }
        public string Description { get; set; }
    }
}

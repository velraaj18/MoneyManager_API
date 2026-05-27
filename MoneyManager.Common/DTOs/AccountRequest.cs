using System;

namespace MoneyManager.DTO
{
    public class AccountRequest
    {
        public string AccountName { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
    }
}

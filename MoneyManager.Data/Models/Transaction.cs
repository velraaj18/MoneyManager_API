using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionUID { get; set; }
        [ForeignKey("Category")]
        public int CategoryUID { get; set; }
        [ForeignKey("Account")]
        public int AccountUID { get; set; }
        [ForeignKey("User")]
        public int UserId {get; set;}
        public DateOnly Date { get; set; }
        public string Description { get; set; }
        [Precision(18, 2)]
        public decimal Amount { get; set; }
        public Category Category { get; set; }
        public Account Account { get; set; }
        public User User {get; set;}
    }
}

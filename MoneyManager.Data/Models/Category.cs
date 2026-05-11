using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MoneyManager.Common.Enums;

namespace MoneyManager.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryUID {get; set;}

        [Required]
        [StringLength(25)]
        public string CategoryName {get; set;}

        public TransactionTypeCode TransactionType { get; set; }

        public string Description {get; set;}
    }
}

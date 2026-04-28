using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountUID {get; set;}

        [Required]
        [StringLength(25)]
        public string AccountName {get; set;}

        public string Description {get; set;}
    }
}

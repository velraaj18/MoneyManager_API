using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyManager.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserUID {get; set;}
        [Required]
        [StringLength(100)]
        public string Email {get; set;}
        [Required]
        public string PasswordHash {get; set;}
    }
}

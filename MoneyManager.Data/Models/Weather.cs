using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.Models
{
    public class Weather
    {
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string City { get; set; }
    
        [Range(-100, 100)]
        public int Temperature { get; set; }
    }
}

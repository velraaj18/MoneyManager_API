using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyManager.DTO
{
    public class WeatherRequest
    {
        [Required]
        [StringLength(25)]
        public string City {get; set;}

        [Range(-100, 100)]
        public int Temperature { get; set; }
    }
}

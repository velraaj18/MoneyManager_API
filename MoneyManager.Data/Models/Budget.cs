using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MoneyManager.Models;

public class Budget
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BudgetUID { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    [Precision(18, 2)]
    [Required]
    public decimal SpendLimit { get; set; }
    [Required]
    public int Month { get; set; }
    [Required]
    public int Year { get; set; }
    [Required]
    public DateTime DateCreated { get; set; }
    public DateTime? ModifiedDate { get; set; }
}

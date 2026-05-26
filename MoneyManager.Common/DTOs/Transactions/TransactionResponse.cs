using MoneyManager.Common.Enums;

public class TransactionResponse
{
    public int TransactionUID { get; set; }
    public DateOnly Date { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public int CategoryId {get; set;}
    public string Category { get; set; }
    public int AccountId {get; set;}
    public string Account { get; set; }
    public TransactionTypeCode TransactionTypeCode { get; set; }
}
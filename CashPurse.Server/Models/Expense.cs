namespace CashPurse.Server.Models;

public class Expense : BaseEntity
{
    public required string Name { get; set; }
    public DateTimeOffset ExpenseDate { get; set; } = DateTimeOffset.Now;

    public string Description { get; set; } = string.Empty;

    public decimal Amount { get; set; }

    public required Currency CurrencyUsed { get; set; }

    public string? ReceiptUrl { get; set; } = string.Empty;

    public string? Notes { get; set; } = string.Empty;

    public required ExpenseType ExpenseType { get; set; }

    public ApplicationUser ExpenseOwner { get; set; } = default!;

    public required string ExpenseOwnerId { get; set; }

    public List<BudgetList> ExpenseBudget { get; set; } = new();

}

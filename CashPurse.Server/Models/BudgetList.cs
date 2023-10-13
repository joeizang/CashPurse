namespace CashPurse.Server.Models;

public class BudgetList : BaseEntity
{
    public required string ListName { get; set; }

    public string Description { get; set; } = string.Empty;

    public List<BudgetListItem> BudgetItems { get; set; } = new();

    public Guid? ExpenseId { get; set; }

    public Expense? Expense { get; set; }

    public required string OwnerId { get; set; } = string.Empty;

    public ApplicationUser Owner { get; set; } = default!;
}

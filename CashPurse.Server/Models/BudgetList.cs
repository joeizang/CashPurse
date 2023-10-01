namespace CashPurse.Server.Models;

public class BudgetList : BaseEntity
{
    public required string ListName { get; set; }

    public string Description { get; set; } = string.Empty;

    public required List<BudgetListItem> BudgetItems { get; set; } = new();

    public Guid OwnerExpenseId { get; set; }

    public string OwnerId { get; set; } = string.Empty;
}

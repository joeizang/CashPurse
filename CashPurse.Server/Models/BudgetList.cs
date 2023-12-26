namespace CashPurse.Server.Models;

public class BudgetList : BaseEntity
{
    public required string ListName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public List<BudgetListItem> BudgetItems { get; set; } = [];

    // public required string OwnerId { get; set; } = string.Empty;
    //
    // public ApplicationUser Owner { get; set; } = default!;
}

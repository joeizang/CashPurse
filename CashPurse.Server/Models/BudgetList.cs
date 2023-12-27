namespace CashPurse.Server.Models;

public class BudgetList : BaseEntity
{
    public required string ListName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool CloseList { get; set; }

    public List<BudgetListItem> BudgetItems { get; set; } = [];

    // public required string OwnerId { get; set; } = string.Empty;
    //
    // public ApplicationUser Owner { get; set; } = default!;
    public Expense MapBudgetListItemsToExpenses(ExpenseType expenseType,
        Currency currency, BudgetListItem budgetListItem)
    {
        var expense = new Expense
        {
            Name = budgetListItem.Name,
            Quantity = budgetListItem.Quantity,
            Amount = budgetListItem.Price,
            ExpenseType = expenseType,
            CurrencyUsed = currency,
            Description = budgetListItem.Description,
            CreatedAt = budgetListItem.CreatedAt,
            ListId = budgetListItem.BudgetListId
        };
        return expense;
    }
}

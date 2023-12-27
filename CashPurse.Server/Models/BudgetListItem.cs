namespace CashPurse.Server.Models;

public class BudgetListItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; private set; }

    public decimal UnitPrice { get; set; }

    public double Quantity { get; set; }

    public string Description { get; set; } = string.Empty;
    
    public Guid BudgetListId { get; set; }

    public bool ConvertedToExpense { get; private set; }

    public void UpdateBudgetListItemStatus(Guid expenseId)
    {
        if(expenseId == Guid.Empty) return;
        ConvertedToExpense = true;
    }

    public void CalculateItemPrice()
    {
        Price = UnitPrice * (decimal)Quantity;
    }
}

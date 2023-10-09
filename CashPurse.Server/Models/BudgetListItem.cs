namespace CashPurse.Server.Models;

public class BudgetListItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; private set; }

    public decimal UnitPrice { get; set; }

    public double Quantity { get; set; }

    public string Description { get; set; } = string.Empty;
    
    public Guid BudgetListId { get; set; }

    public void CalculateItemPrice()
    {
        Price = UnitPrice * (decimal)Quantity;
    }
}

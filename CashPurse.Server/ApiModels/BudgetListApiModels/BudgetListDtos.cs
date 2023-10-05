using NodaTime;

namespace CashPurse.Server.ApiModels.BudgetListApiModels;

public record BudgetListItemModel(Guid ItemId, string Name, double Quantity, decimal Price, 
    decimal UnitPrice, string Description);
public record CreateBudgetListItemModel(string ListName, double Quantity, decimal Price,
    decimal UnitPrice, string Description);

public record BudgetListModel(Guid ListId, string Name, string Description, Guid OwnerExpenseId, DateTime CreatedAt,
    IEnumerable<BudgetListItemModel> BudgetItems);

public record BudgetListIndexModel(Guid ListId, string Name, string Description, string OwnerId, int ItemCount,
    IEnumerable<BudgetListItemModel> Items);

public class CreateBudgetListRequest
{
    public required string Name { get; set; }

    public string Description { get; set; } = string.Empty;

    public required List<BudgetListItemModel> Items { get; set; }
}

public record UpdateBudgetListRequest(string ListName, string Description, string ExpenseId,
    IEnumerable<BudgetListItemModel> BudgetItems, Guid BudgetListId);

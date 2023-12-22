using NodaTime;

namespace CashPurse.Server.ApiModels.BudgetListApiModels;

public record BudgetListItemModel(Guid ItemId, string Name, double Quantity, decimal Price, 
    decimal UnitPrice, string Description, DateOnly CreatedAt, Guid? BudgetListId);
public record CreateBudgetListItemRequest(string ListName, double Quantity, decimal Price,
    decimal UnitPrice, string Description);
public record UpdateBudgetListItemRequest(Guid ItemId, string Name, double Quantity, decimal Price, 
    decimal UnitPrice, string Description, Guid BudgetListId);

public record BudgetListModel(Guid ListId, string Name, string Description, Guid OwnerExpenseId, DateOnly CreatedAt,
    IEnumerable<BudgetListItemModel> BudgetItems);

public record BudgetListIndexModel(Guid ListId, string Name, string Description, string OwnerId, int ItemCount,
    IEnumerable<BudgetListItemModel> Items);

public record CreateBudgetListRequest(string Name, string Description);

public record UpdateBudgetListRequest(string ListName, string? Description, string? ExpenseId,
    IEnumerable<BudgetListItemModel> BudgetItems, Guid BudgetListId, string? BudgetListOwnerId);

using CashPurse.Server.ApiModels.ExpensesApiModels;
using NodaTime;

namespace CashPurse.Server.ApiModels.BudgetListApiModels;

public record BudgetListItemModel(Guid ItemId, string Name, double Quantity, decimal Price, 
    decimal UnitPrice, string Description, DateOnly CreatedAt, Guid? BudgetListId);
public record CreateBudgetListItemRequest(string ListName, double Quantity, decimal Price,
    decimal UnitPrice, string Description);
public record UpdateBudgetListItemRequest(Guid ItemId, string Name, double Quantity, decimal UnitPrice, string Description, Guid BudgetListId);

public record BudgetListModel(Guid ListId, string Name, string Description, DateOnly CreatedAt,
    IEnumerable<BudgetListItemModel> BudgetItems, List<ExpenseIndexModel> Expenses);

public record BudgetListIndexModel(Guid ListId, string Name, string Description, string OwnerId, int ItemCount,
    IEnumerable<BudgetListItemModel> Items);

public record CreateBudgetListRequest(string Name, string Description);

public record UpdateBudgetListRequest(string ListName, string? Description, string? ExpenseId,
    IEnumerable<BudgetListItemModel> BudgetItems, Guid BudgetListId, string? BudgetListOwnerId);

public record StrikeItemOffCurrentListRequest(Guid BudgetListItemId, Guid BudgetListId, string Name,
    double Quantity, decimal Price, decimal UnitPrice, string Description, DateOnly CreatedAt);

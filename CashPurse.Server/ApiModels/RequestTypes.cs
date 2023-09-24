using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Models;
using NodaTime;

namespace CashPurse.Server.ApiModels;

public record CursorPagedRequest(DateTime Cursor);

public record CreateExpenseRequest(string Name, string Description, decimal Amount, DateTime ExpenseDate,
    ExpenseType ExpenseType, Currency CurrencyUsed, string ExpenseOwnerId, string Notes);

public record UpdateExpenseRequest(string Name, string Description, decimal Amount, DateTime ExpenseDate,
    ExpenseType ExpenseType, Currency CurrencyUsed, string ExpenseOwnerId, string Notes);

public record UpdateBudgetListModel(string ListName, string Description, string ExpenseId,
    IEnumerable<BudgetListItemModel> BudgetItems, Guid BudgetListId);

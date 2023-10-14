using System.Linq;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.CompiledEFQueries
{
    public static class CompiledQueries
    {
        public static readonly Func<CashPurseDbContext, string, int>
       GetNnumberOfUserExpensesAsync =
           EF.CompileQuery(
               (CashPurseDbContext context, string userId) =>
                   context.Expenses
                       .AsNoTracking().Count(e => e.ExpenseOwnerId == userId));

        public static readonly Func<CashPurseDbContext, string, Guid, ExpenseIndexModel>
            GetExpenseById =
                EF.CompileQuery((CashPurseDbContext context, string userId, Guid expenseId) => 
                        context.Expenses.AsNoTracking()
                        .Where(e => e.Id.Equals(expenseId))
                        .Where(e => e.ExpenseOwnerId.Equals(userId))
                        .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                        .Single());


        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetUserExpensesAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, string, DateOnly, IAsyncEnumerable<ExpenseIndexModel>>
            GetCursorPagedUserExpensesAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId, DateOnly cursor) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= cursor)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));
        
        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetCursorPagedUserExpensesWithoutCursorAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, DateOnly, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetCursorPagedExpensesFilteredByExpenseTypeAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, DateOnly cursor, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseType)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= cursor)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));
        
        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetCursorPagedExpensesFilteredByExpenseTypeWithoutCursorAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseType)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, int, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetExpensesFilteredByExpenseTypeAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, int skipValue, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseType)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Skip(skipValue)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, int, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetUserExpensesFilteredByCurrencyUsedAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, int skipValue, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.CurrencyUsed)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Skip(skipValue)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, DateOnly, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetCursorPagedUserExpensesFilteredByCurrencyUsedAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, DateOnly cursor, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.CurrencyUsed)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= cursor)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));
        
        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetCursorPagedUserExpensesFilteredByCurrencyUsedWithoutCursorAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.CurrencyUsed)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!, userId))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, string, IEnumerable<ExpenseDashBoardSummary>>
            GetExpenseTotalForLast30DaysAsync =
                EF.CompileQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= DateOnly.FromDateTime(DateTime.Now.ToLocalTime().AddDays(-30)))
                            .Select(e => new ExpenseDashBoardSummary(e.Amount, e.CurrencyUsed))
                            .Take(30));

        public static readonly Func<CashPurseDbContext, string, int, IAsyncEnumerable<ExpenseDashBoardSummary>>
            GetMeanSpendOverSpecifiedPeriod =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId, int days) =>
                        context.Expenses.AsNoTracking()
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= DateOnly.FromDateTime(DateTime.Now.ToLocalTime().AddDays(-days)))
                            .GroupBy(e => e.CurrencyUsed)
                            .Select(e => 
                                new ExpenseDashBoardSummary(e.Average(e => e.Amount), e.Key))
                );
        // BUDGETLIST QUERIES //
        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<BudgetListModel>>
            GetUserBudgetLists =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.BudgetLists.AsNoTracking()
                            .OrderBy(b => b.CreatedAt)
                            .Where(b => b.OwnerId == userId)
                            .Select(b => new BudgetListModel(b.Id, b.ListName, b.Description,
                                b.ExpenseId ?? Guid.Empty, b.CreatedAt,
                                b.BudgetItems.Select(x => new BudgetListItemModel(
                                    x.Id, x.Description, x.Quantity, x.Price, x.UnitPrice,
                                    x.Description, x.CreatedAt))))
                            .Take(7)
                );

        public static readonly Func<CashPurseDbContext, DateOnly, string, IAsyncEnumerable<BudgetListModel>>
            GetCursorPagedUserBudgetLists =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, DateOnly cursor, string userId) =>
                        context.BudgetLists.AsNoTracking()
                            .OrderBy(b => b.CreatedAt)
                            .Where(b => b.OwnerId == userId)
                            .Where(b => b.CreatedAt >= cursor)
                            .Select(b => new BudgetListModel(b.Id, b.ListName, b.Description,
                                b.ExpenseId ?? Guid.Empty, b.CreatedAt,
                                b.BudgetItems.Select(x => 
                                    new BudgetListItemModel(x.Id, x.Description,
                                        x.Quantity, x.Price, x.UnitPrice, x.Description, x.CreatedAt))))
                            .Take(7)
                );
        
        public static readonly Func<CashPurseDbContext, Guid, IAsyncEnumerable<BudgetListModel>>
            GetCursorPagedUserBudgetListsWithoutCursor =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, Guid expenseId) =>
                        context.BudgetLists.AsNoTracking()
                            .OrderBy(b => b.CreatedAt)
                            .Where(b => b.ExpenseId == expenseId)
                            // .Where(b => b.CreatedAt >= cursor)
                            .Select(b => new BudgetListModel(b.Id, b.ListName, b.Description,
                                b.ExpenseId ?? Guid.Empty, b.CreatedAt,
                                b.BudgetItems.Select(x => new BudgetListItemModel(
                                    x.Id, x.Name, x.Quantity, x.Price, x.UnitPrice, x.Description, x.CreatedAt))))
                            .Take(7)
                );
        
        public static readonly Func<CashPurseDbContext, Guid, IAsyncEnumerable<BudgetListItemModel>>
            GetPagedBudgetListItems =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, Guid budgetListId) =>
                        context.BudgetListItems.AsNoTracking()
                            .OrderBy(b => b.CreatedAt)
                            .Where(b => b.BudgetListId == budgetListId)
                            .Select(b => new BudgetListItemModel(b.Id, b.Description, b.Quantity, b.Price, b.UnitPrice,
                                b.Description, b.CreatedAt))
                            .Take(7)
                );
        
        public static readonly Func<CashPurseDbContext, DateOnly, Guid, IAsyncEnumerable<BudgetListItemModel>>
            GetCursorPagedBudgetListItems =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, DateOnly cursor, Guid budgetListId) =>
                        context.BudgetListItems.AsNoTracking()
                            .OrderBy(b => b.CreatedAt)
                            .Where(b => b.BudgetListId == budgetListId)
                            .Where(b => b.CreatedAt >= cursor)
                            .Select(b => new BudgetListItemModel(b.Id, b.Description, b.Quantity, b.Price, b.UnitPrice,
                                b.Description, b.CreatedAt))
                            .Take(7)
                );

        public static readonly Func<CashPurseDbContext, Guid, IEnumerable<BudgetList>>
            GetUserBudgetListsForCount =
                EF.CompileQuery(
                    (CashPurseDbContext context, Guid id) =>
                        context.BudgetLists
                            .AsNoTracking()
                            .Where(b => b.ExpenseId == id)
                );
    }
}

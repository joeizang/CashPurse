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
                       .AsNoTracking().Count(e => e.ExpenseOwner.Id == userId));


        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetUserExpensesAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwner.Id == userId)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!)));

        public static readonly Func<CashPurseDbContext, string, DateTimeOffset, IAsyncEnumerable<ExpenseIndexModel>>
            GetUserExpensesCursorPagedAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId, DateTimeOffset cursor) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwner.Id == userId)
                            .Where(e => e.ExpenseDate >= cursor)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!)));

        public static readonly Func<CashPurseDbContext, int, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetUserExpensesFilteredByExpenseTypeAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, int skipValue, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.ExpenseType)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Skip(skipValue)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, int, string, IAsyncEnumerable<ExpenseIndexModel>>
            GetUserExpensesFilteredByCurrencUsedAsync =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, int skipValue, string userId) =>
                        context.Expenses.AsNoTracking()
                            .OrderBy(e => e.CurrencyUsed)
                            .ThenBy(e => e.ExpenseDate)
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Skip(skipValue)
                            .Select(e => new ExpenseIndexModel(e.Name, e.Description, e.Amount, e.ExpenseDate, e.Id,
                                e.CurrencyUsed, e.ExpenseType, e.Notes!))
                            .Take(7));

        public static readonly Func<CashPurseDbContext, string, IEnumerable<ExpenseDashBoardSummary>>
            GetExpenseTotalForLast30DaysAsync =
                EF.CompileQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.Expenses.AsNoTracking()
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= DateTime.Now.AddDays(-30))
                            .Select(e => new ExpenseDashBoardSummary(e.Amount, e.CurrencyUsed)));

        public static readonly Func<CashPurseDbContext, string, int, IAsyncEnumerable<ExpenseDashBoardSummary>>
            GetMeanSpendOverSpecifiedPeriod =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId, int days) =>
                        context.Expenses.AsNoTracking()
                            .Where(e => e.ExpenseOwnerId == userId)
                            .Where(e => e.ExpenseDate >= DateTime.Now.AddDays(-days))
                            .GroupBy(e => e.CurrencyUsed)
                            .Select(e => new ExpenseDashBoardSummary(e.Average(e => e.Amount), e.Key))
                );
        // BUDGETLIST QUERIES //
        public static readonly Func<CashPurseDbContext, string, IAsyncEnumerable<BudgetListModel>>
            GetUserBudgetLists =
                EF.CompileAsyncQuery(
                    (CashPurseDbContext context, string userId) =>
                        context.BudgetLists.AsNoTracking()
                            .OrderBy(b => b.CreatedAt)
                            .Where(b => b.Expense.ExpenseOwnerId == userId)
                            .Select(b => new BudgetListModel(b.Id, b.ListName, b.Description,
                                b.Expense.ExpenseOwnerId, 
                                b.BudgetItems.Select(x => new BudgetListItemModel(
                                    x.Id, x.Description, x.Quantity, x.Price, x.UnitPrice, x.Description))))
                );

        public static readonly Func<CashPurseDbContext, string, Guid, IEnumerable<BudgetList>>
            GetUserBudgetListsForCount =
                EF.CompileQuery(
                    (CashPurseDbContext context, string userId, Guid id) =>
                        context.BudgetLists
                            .AsNoTracking()
                            .Where(b => b.Expense.ExpenseOwnerId == userId)
                            .Where(b => b.Id == id)
                );
    }
}

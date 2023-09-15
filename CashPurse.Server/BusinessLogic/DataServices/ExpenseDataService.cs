using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.CompiledEFQueries;
using CashPurse.Server.Data;
using CashPurse.Server.Models;

namespace CashPurse.Server.BusinessLogic.DataServices;

public static class ExpenseDataService
{

    public static int TotalExpenses { get; set; }

    public static int TotalCount(CashPurseDbContext _context, string userId)
    {
        return CompiledQueries.GetNnumberOfUserExpensesAsync(_context, userId);
    }

    public static async Task<PagedResult<ExpenseIndexModel>> GetUserExpenses(CashPurseDbContext _context,
        string userId, int pageNumber = 1)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(_context, userId);
        await foreach (var each in CompiledQueries.GetUserExpensesAsync(_context, userId))
        {
            expenses.Add(each);
        }

        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, pageNumber,
                (int)Math.Ceiling(TotalExpenses / (double)7), 7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public static async Task<CursorPagedResult<List<ExpenseIndexModel>>> GetCursorPagedUserExpenses(CashPurseDbContext _context,
        string userId, DateTimeOffset cursor)
    {
        var expenses = new List<ExpenseIndexModel>();
        await foreach (var each in CompiledQueries.GetCursorPagedUserExpensesAsync(_context, userId, cursor))
        {
            expenses.Add(each);
        }

        if(expenses.Count == 0) return new CursorPagedResult<List<ExpenseIndexModel>>(DateTimeOffset.UtcNow, expenses);
        return new CursorPagedResult<List<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);
    }

    public static async Task<PagedResult<ExpenseIndexModel>> GetExpenseTypeFilteredExpenses(CashPurseDbContext _context,
        string userId, int pageNumber)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(_context, userId);
        await foreach (var each in CompiledQueries.GetExpensesFilteredByExpenseTypeAsync(_context, pageNumber, userId))
        {
            expenses.Add(each);
        }
        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, pageNumber,
                (int)Math.Ceiling(TotalExpenses / (double)7), 7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public static async Task<CursorPagedResult<IEnumerable<ExpenseIndexModel>>> GetCursorPagedTypeFilteredExpenses(CashPurseDbContext _context,
        string userId, DateTimeOffset cursor)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(_context, userId);
        await foreach (var each in CompiledQueries.GetCursorPagedExpensesFilteredByExpenseTypeAsync(_context, cursor, userId))
        {
            expenses.Add(each);
        }
        return new CursorPagedResult<IEnumerable<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);
    }

    public static async Task<PagedResult<ExpenseIndexModel>> GetCurrencyUsedFilteredExpenses(CashPurseDbContext _context, string userId, int pageNumber)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(_context, userId);
        await foreach (var each in CompiledQueries.GetUserExpensesFilteredByCurrencyUsedAsync(_context, pageNumber, userId))
        {
            expenses.Add(each);
        }
        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, pageNumber,
                (int)Math.Ceiling(TotalExpenses / (double)7), 7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public static async Task<CursorPagedResult<IEnumerable<ExpenseIndexModel>>> GetCurrencyUsedCursorPagedFilteredExpenses(CashPurseDbContext _context, 
        string userId, DateTimeOffset cursor)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(_context, userId);
        await foreach (var each in CompiledQueries.GetCursorPagedUserExpensesFilteredByCurrencyUsedAsync(_context, cursor, userId))
        {
            expenses.Add(each);
        }
        return new CursorPagedResult<IEnumerable<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);
    }

    public static decimal GetExpenseTotalForLastMonth(CashPurseDbContext _context, string userId)
    {
        var result = CompiledQueries.GetExpenseTotalForLast30DaysAsync(_context, userId);
        var actualTotal = 0m;
        foreach (var each in result)
        {
            if (each.CurrencyUsed == Currency.USD)
            {
                actualTotal += each.Amount * 750;
            }
            else if (each.CurrencyUsed == Currency.EUR)
            {
                actualTotal += each.Amount * 900;
            }
            else if (each.CurrencyUsed == Currency.GBP)
            {
                actualTotal += each.Amount * 1050;
            }
            else if (each.CurrencyUsed == Currency.NGN)
            {
                actualTotal += each.Amount;
            }
        }
        return actualTotal;
    }

    public static async Task<decimal> GetMeanSpendByDays(CashPurseDbContext _context, string userId, int days)
    {
        var result = 0m;
        var counter = 0;
        await foreach (var item in CompiledQueries.GetMeanSpendOverSpecifiedPeriod(_context, userId, days))
        {
            var temp = item.CurrencyUsed switch
            {
                Currency.USD => item.Amount * 799,
                Currency.EUR => item.Amount * 1046,
                Currency.GBP => item.Amount * 1222,
                _ => item.Amount
            };
            result += temp;
            counter++;
        }
        return result;
    }
}

public interface IExpenseDataService
{
    decimal GetExpenseTotalForLastMonth(string userId);
    Task<PagedResult<ExpenseIndexModel>> GetUserExpenses(string userId, int pageNumber = 1);

    Task<PagedResult<ExpenseIndexModel>> GetCurrencyUsedFilteredExpenses(string userId, int pageNumber);

    Task<PagedResult<ExpenseIndexModel>> GetExpenseTypeFilteredExpenses(string userId, int pageNumber);

    Task<decimal> GetMeanSpendByDays(string userId, int days);
}

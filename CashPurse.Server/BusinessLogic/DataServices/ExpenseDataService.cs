using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.CompiledEFQueries;
using CashPurse.Server.Data;
using CashPurse.Server.Models;

namespace CashPurse.Server.BusinessLogic.DataServices;

public class ExpenseDataService : IExpenseDataService
{
    private readonly CashPurseDbContext _context;

    public ExpenseDataService(CashPurseDbContext context)
    {
        _context = context;
    }

    public int TotalExpenses { get; set; }

    public int TotalCount(string userId)
    {
        return CompiledQueries.GetNnumberOfUserExpensesAsync(_context, userId);
    }

    public async Task<PagedResult<ExpenseIndexModel>> GetUserExpenses(string userId, int pageNumber = 1)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(userId);
        await foreach (var each in CompiledQueries.GetUserExpensesAsync(_context, userId))
        {
            expenses.Add(each);
        }

        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, pageNumber,
                (int)Math.Ceiling(TotalExpenses / (double)7), 7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public async Task<CursorPagedResult<List<ExpenseIndexModel>>> GetUserExpensesCursorPaged(string userId,
        DateTimeOffset cursor)
    {
        var expenses = new List<ExpenseIndexModel>();
        await foreach (var each in CompiledQueries.GetUserExpensesCursorPagedAsync(_context, userId, cursor))
        {
            expenses.Add(each);
        }

        return new CursorPagedResult<List<ExpenseIndexModel>>(expenses[^1].ExpenseDate, expenses);
    }

    public async Task<PagedResult<ExpenseIndexModel>> GetExpenseTypeFilteredExpenses(string userId, int pageNumber)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(userId);
        await foreach (var each in CompiledQueries.GetUserExpensesFilteredByExpenseTypeAsync(_context, pageNumber, userId))
        {
            expenses.Add(each);
        }
        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, pageNumber,
                (int)Math.Ceiling(TotalExpenses / (double)7), 7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public async Task<PagedResult<ExpenseIndexModel>> GetCurrencyUsedFilteredExpenses(string userId, int pageNumber)
    {
        var expenses = new List<ExpenseIndexModel>();
        TotalExpenses = TotalCount(userId);
        await foreach (var each in CompiledQueries.GetUserExpensesFilteredByCurrencUsedAsync(_context, pageNumber, userId))
        {
            expenses.Add(each);
        }
        return new PagedResult<ExpenseIndexModel>(expenses, TotalExpenses, pageNumber, pageNumber,
                (int)Math.Ceiling(TotalExpenses / (double)7), 7, pageNumber < (int)Math.Ceiling(TotalExpenses / (double)7), pageNumber > 1);
    }

    public decimal GetExpenseTotalForLastMonth(string userId)
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

    public async Task<decimal> GetMeanSpendByDays(string userId, int days)
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

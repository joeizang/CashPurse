using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.CompiledEFQueries;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.DataServices;

public interface IBudgetListDataService
{
    Task<IEnumerable<BudgetListModel>> GetUserBudgetLists(string userId);

    Task AddNewBudgetList(BudgetList entity);
    Task<bool> UpdateBudgetList(BudgetList entity);
    Task DeleteBudgetList(BudgetList entity);
}

public class BudgetListDataService : IBudgetListDataService
{
    private readonly CashPurseDbContext _context;

    public BudgetListDataService(CashPurseDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BudgetListModel>> GetUserBudgetLists(string userId)
    {
        var results = new List<BudgetListModel>();
        await foreach (var budgetList in CompiledQueries.GetUserBudgetLists(_context, userId))
        {
            results.Add(budgetList);
        }

        return results;
    }

    public int CountBudgetListItems(string userId, Guid id)
    {
        var result = CompiledQueries
            .GetUserBudgetListsForCount(_context, userId, id)
            .SingleOrDefault()!.BudgetItems.Count;
        return result;
    }

    public async Task AddNewBudgetList(BudgetList entity)
    {
        _context.BudgetLists.Add(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task<bool> UpdateBudgetList(BudgetList entity)
    {
        try
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task DeleteBudgetList(BudgetList entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}

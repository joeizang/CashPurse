using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.CompiledEFQueries;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace CashPurse.Server.BusinessLogic.DataServices;

public static class BudgetListDataService
{

    public static async Task<List<BudgetListModel>> GetUserBudgetLists(CashPurseDbContext _context, string userId, DateTime cursor)
    {
        var results = new List<BudgetListModel>();
        await foreach (var budgetList in CompiledQueries.GetCursorPagedUserBudgetLists(_context, cursor, userId))
        {
            results.Add(budgetList);
        }

        return results;
    }

    public static int CountBudgetListItems(CashPurseDbContext _context, string userId, Guid id)
    {
        var result = CompiledQueries
            .GetUserBudgetListsForCount(_context, userId, id)
            .SingleOrDefault()!.BudgetItems.Count;
        return result;
    }

    public static async Task AddNewBudgetList(CashPurseDbContext _context,BudgetList entity)
    {
        _context.BudgetLists.Add(entity);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public static async Task<bool> UpdateBudgetList(CashPurseDbContext _context, BudgetList entity)
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

    public static async Task DeleteBudgetList(CashPurseDbContext _context, BudgetList entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}

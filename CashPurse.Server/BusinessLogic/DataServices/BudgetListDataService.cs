using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.CompiledEFQueries;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace CashPurse.Server.BusinessLogic.DataServices;

public static class BudgetListDataService
{

    public static async Task<CursorPagedResult<List<BudgetListModel>>> GetCursorPagedUserBudgetLists(CashPurseDbContext _context,
        Guid ownerExpenseId, DateTime cursor)
    {
        var results = new List<BudgetListModel>();

        await foreach (var budgetList in CompiledQueries.GetCursorPagedUserBudgetLists(_context,
                           cursor, ownerExpenseId))
        {
            results.Add(budgetList);
        }

        return new CursorPagedResult<List<BudgetListModel>>(results[^1].CreatedAt, results);
    }
    
    public static async Task<PagedResult<BudgetListModel>> GetUserBudgetLists(CashPurseDbContext _context,
        Guid ownerExpenseId)
    {
        var results = new List<BudgetListModel>();

        await foreach (var budgetList in CompiledQueries.GetUserBudgetLists(_context, ownerExpenseId))
        {
            results.Add(budgetList);
        }

        return new PagedResult<BudgetListModel>(results, results.Count, 
            1, 7, 
            (int)Math.Ceiling(results.Count / (double)7), 1,
            7 < (int)Math.Ceiling(results.Count / (double)7));
    }

    public static int CountBudgetListItems(CashPurseDbContext _context, Guid id)
    {
        var result = CompiledQueries
            .GetUserBudgetListsForCount(_context, id)
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
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return true;
    }

    public static async Task DeleteBudgetList(CashPurseDbContext _context, BudgetList entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}

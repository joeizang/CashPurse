using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.BusinessLogic.Exceptions;
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

    public static async Task AddNewBudgetListItem(CashPurseDbContext context, BudgetListItem entity)
    {
        context.BudgetListItems.Add(entity);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public static async Task UpdateBudgetListItem(CashPurseDbContext context, UpdateBudgetListItemRequest model)
    {
        var target = await context.BudgetListItems.FindAsync(model.ItemId).ConfigureAwait(false);
        if(target is null) throw new BudgetListOrItemNotFound("Item not found.");
        target.Name = model.Name;
        target.Description = model.Description;
        target.UnitPrice = model.UnitPrice;
        target.Quantity = model.Quantity;
        target.CalculateItemPrice();
        target.UpdatedAt = DateTime.UtcNow.ToLocalTime();
        
        context.Entry(target).State = EntityState.Modified;
        await context.SaveChangesAsync().ConfigureAwait(false); 
    }
    
    public static async Task<PagedResult<BudgetListItemModel>> GetBudgetListItems(
        CashPurseDbContext context, Guid budgetListId)
    {
        var results = new List<BudgetListItemModel>();

        await foreach (var budgetListItem in CompiledQueries.GetPagedBudgetListItems(context, budgetListId))
        {
            results.Add(budgetListItem);
        }

        return new PagedResult<BudgetListItemModel>(results, results.Count, 
            1, 7, 
            (int)Math.Ceiling(results.Count / (double)7), 1,
            7 < (int)Math.Ceiling(results.Count / (double)7));
    }
    
    public static async Task<CursorPagedResult<List<BudgetListItemModel>>> GetCursorPagedBudgetListItems(
        CashPurseDbContext context, Guid budgetListId, DateTime cursor)
    {
        var results = new List<BudgetListItemModel>();

        await foreach (var budgetListItem in CompiledQueries.GetCursorPagedBudgetListItems(context, cursor, budgetListId))
        {
            results.Add(budgetListItem);
        }

        return new CursorPagedResult<List<BudgetListItemModel>>(results[^1].CreatedAt, results);
    }
}

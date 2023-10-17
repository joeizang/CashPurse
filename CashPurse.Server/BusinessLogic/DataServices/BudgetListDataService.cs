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
        string ownerExpenseId, DateOnly cursor)
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
        string ownerId)
    {
        // var results = new List<BudgetListModel>();

        // await foreach (var budgetList in CompiledQueries.GetUserBudgetLists(_context, ownerId))
        // {
        //     results.Add(budgetList);
        // }
        var results = await _context.BudgetLists.AsNoTracking()
                .OrderBy(b => b.CreatedAt)
                .Where(b => b.OwnerId == ownerId)
                .Select(b => new BudgetListModel(b.Id, b.ListName, b.Description,
                    b.ExpenseId ?? Guid.Empty, b.CreatedAt,
                    b.BudgetItems.Select(x => new BudgetListItemModel(
                        x.Id, x.Description, x.Quantity, x.Price, x.UnitPrice,
                        x.Description, x.CreatedAt))))
                .Take(7).ToListAsync(CancellationToken.None).ConfigureAwait(false);

        return new PagedResult<BudgetListModel>(results, results.Count, 
            1, 7, 
            (int)Math.Ceiling(results.Count / (double)7), 1,
            7 < (int)Math.Ceiling(results.Count / (double)7));
    }
    
    public static BudgetListModel GetBudgetListById(Guid id, CashPurseDbContext _context)
    {
        var result = CompiledQueries.GetBudgetListById(_context, id) ?? throw new BudgetListOrItemNotFound("Budget list not found.");
        return result;
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
        entity.CalculateItemPrice();
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
        target.UpdatedAt = DateOnly.FromDateTime(DateTime.Today);
        
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
        CashPurseDbContext context, Guid budgetListId, DateOnly cursor)
    {
        var results = new List<BudgetListItemModel>();

        await foreach (var budgetListItem in CompiledQueries.GetCursorPagedBudgetListItems(context, cursor, budgetListId))
        {
            results.Add(budgetListItem);
        }

        return new CursorPagedResult<List<BudgetListItemModel>>(results[^1].CreatedAt, results);
    }
}

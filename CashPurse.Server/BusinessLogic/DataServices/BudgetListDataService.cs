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

    public static async Task<CursorPagedResult<List<BudgetListModel>>> CursorPagedUserBudgetLists(CashPurseDbContext context,
        string ownerExpenseId, DateOnly cursor)
    {
        var results = new List<BudgetListModel>();

        await foreach (var budgetList in CompiledQueries.GetCursorPagedUserBudgetLists(context,
                           cursor, ownerExpenseId))
        {
            results.Add(budgetList);
        }

        return new CursorPagedResult<List<BudgetListModel>>(results[^1].CreatedAt, results);
    }
    
    public static async Task<PagedResult<BudgetListModel>> UserBudgetLists(CashPurseDbContext context)
    {
        // var results = new List<BudgetListModel>();

        // await foreach (var budgetList in CompiledQueries.GetUserBudgetLists(_context, ownerId))
        // {
        //     results.Add(budgetList);
        // }
        var results = await context.BudgetLists.AsNoTracking()
                .OrderBy(b => b.CreatedAt)
                // .Where(b => b.OwnerId == ownerId)
                .Select(b => new BudgetListModel(b.Id, b.ListName, b.Description,
                    b.ExpenseId ?? Guid.Empty, b.CreatedAt,
                    b.BudgetItems.Select(x => new BudgetListItemModel(
                        x.Id, x.Description, x.Quantity, x.Price, x.UnitPrice,
                        x.Description, x.CreatedAt, b.Id))))
                .Take(7).ToListAsync(CancellationToken.None).ConfigureAwait(false);

        return new PagedResult<BudgetListModel>(results, results.Count, 
            1, 7, 
            (int)Math.Ceiling(results.Count / (double)7), 1,
            7 < (int)Math.Ceiling(results.Count / (double)7));
    }
    
    public static BudgetListModel BudgetListById(Guid id, CashPurseDbContext context)
    {
        var result = CompiledQueries.GetBudgetListById(context, id) ?? 
                     throw new BudgetListOrItemNotFound("Budget list not found.");
        return result;
    }

    public static int CountBudgetListItems(CashPurseDbContext context, Guid id)
    {
        var result = CompiledQueries
            .GetUserBudgetListsForCount(context, id)
            .SingleOrDefault()!.BudgetItems.Count;
        return result;
    }

    public static async Task AddNewBudgetList(CashPurseDbContext context,BudgetList entity)
    {
        context.BudgetLists.Add(entity);
        await context.SaveChangesAsync().ConfigureAwait(false);
    }

    public static async Task<bool> UpdateBudgetList(CashPurseDbContext context, BudgetList entity)
    {
        int retryCounter = 0;
        var transaction = await context.Database.BeginTransactionAsync().ConfigureAwait(false);
        while (true)
        {
            try
            {
                context.Entry(entity).State = EntityState.Modified;
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (retryCounter >= 5)
                {
                    throw new Exception($"Failed to update budget list. Core error is {e.Message}", e);
                }

                await transaction.RollbackAsync();
                retryCounter++;
                foreach (var entry in e.Entries)
                {
                    if (entry.Entity is BudgetList)
                    {
                        // get original values when the entity was first loaded
                        var originalValuesLoaded = entry.OriginalValues;
                        // get the current values, as they are in the database right now
                        var databaseValues = await entry.GetDatabaseValuesAsync();
                        // get the values that were sent with the request
                        var clientValues = entry.CurrentValues;
                        
                        // update the original values with the database values
                        originalValuesLoaded.SetValues(databaseValues!);
                    }
                }
            }
        }
    }

    public static async Task DeleteBudgetList(CashPurseDbContext context, BudgetList entity)
    {
        context.Entry(entity).State = EntityState.Modified;
        await context.SaveChangesAsync().ConfigureAwait(false);
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
    
    public static async Task<PagedResult<BudgetListItemModel>> BudgetListItems(
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
    
    public static async Task<CursorPagedResult<List<BudgetListItemModel>>> CursorPagedBudgetListItems(
        CashPurseDbContext context, Guid budgetListId, DateOnly cursor)
    {
        var results = new List<BudgetListItemModel>();

        await foreach (var budgetListItem in CompiledQueries.GetCursorPagedBudgetListItems(context, cursor, budgetListId))
        {
            results.Add(budgetListItem);
        }

        return new CursorPagedResult<List<BudgetListItemModel>>(results[^1].CreatedAt, results);
    }

    public static BudgetListItemModel BudgetListItemById(CashPurseDbContext context, Guid budgetListId, Guid id)
    {
        var result = CompiledQueries.GetBudgetListItemById(context, budgetListId, id);
        return result ?? throw new BudgetListOrItemNotFound("Item not found.");
    }
}

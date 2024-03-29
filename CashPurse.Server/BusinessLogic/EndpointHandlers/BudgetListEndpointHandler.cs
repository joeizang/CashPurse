using System.Security.Claims;
using System.Security.Principal;
using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.BusinessLogic.Exceptions;
using CashPurse.Server.Data;
using CashPurse.Server.MapperConfiguration;
using CashPurse.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.EndpointHandlers;

// TODO: configure and use ILogger to log all exceptions and other successful operations

public static class BudgetListEndpointHandler
{
    private static Task<ApplicationUser?> GetCurrentUser(IPrincipal principal, 
        UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name;
        return userManager.FindByNameAsync(userId!);
    }

    public static async Task<Ok<PagedResult<BudgetListModel>>> HandleGet([FromServices] CashPurseDbContext context)
    {
        var alternative = await BudgetListDataService.UserBudgetLists(context)
            .ConfigureAwait(false);
        return TypedResults.Ok(alternative?.Items.Count == 0 ? alternative : alternative!);
    }
    
    public static async Task<IResult> HandleCursorPagedGet(
        [FromServices] CashPurseDbContext context, DateOnly cursor)
    {
        var alternative = await BudgetListDataService
            .CursorPagedUserBudgetLists(context, string.Empty, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(alternative?.Data.Count == 0 ? alternative : alternative!);
    }
    
    public static IResult HandleGetById([FromServices] CashPurseDbContext context, Guid budgetListId)
    {
        try
        {
            var result = BudgetListDataService.BudgetListById(budgetListId, context);
            return result is not null ? TypedResults.Ok(result) : Results.NotFound("Budget list not found.");
        }
        catch(Exception b)
        {
            return Results.NotFound("Budget list not found.");
        }
    }

    public static async Task<IResult> CreateBudgetList(
        [FromServices] CashPurseDbContext context, [FromBody]CreateBudgetListRequest inputModel)
    {
        try
        {
            var budgetList = BudgetListMapper.MapCreateBudgetList(inputModel);
            await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
            return Results.Created($"budgetlists/{budgetList.Id}", budgetList);
        }
        catch (Exception e)
        {
            return Results.Problem("There was a problem but the problem ain't yours!");
        }
    }

    private static readonly Func<Guid, CashPurseDbContext, Task<BudgetList?>> 
        FetchBudgetListById = async (id, context) =>
    {
        return await context.BudgetLists.FindAsync(id).ConfigureAwait(false);
    };
    
    private static readonly Func<UpdateBudgetListRequest, BudgetList, BudgetList> MapUpdateBudgetList = 
        (inputModel, entity) =>
        {
            entity.ListName = inputModel.ListName;
            entity.Description = inputModel.Description ?? string.Empty;
            return entity;
        };
    private static readonly Func<UpdateBudgetListRequest, CashPurseDbContext, Task<List<BudgetListItem>>> 
        MapUpdateBudgetListItem = async (inputModel, context) =>
        {
            return await context.BudgetListItems
                .Where(b => b.BudgetListId == inputModel.BudgetListId)
                .ToListAsync(CancellationToken.None)
                .ConfigureAwait(false);
        };
    public static async Task<NoContent> UpdateBudgetList(
        [FromServices] CashPurseDbContext context, Guid budgetListId,
        [FromBody]UpdateBudgetListRequest inputModel
    ) 
    {
        var tempList = await FetchBudgetListById(budgetListId, context).ConfigureAwait(false);
        var budgetList = MapUpdateBudgetList(inputModel, tempList!);
        var budgetListItems = await MapUpdateBudgetListItem(inputModel, context).ConfigureAwait(false);
        budgetList.BudgetItems = budgetListItems;
        await BudgetListDataService.UpdateBudgetList(context, budgetList).ConfigureAwait(false);
        return TypedResults.NoContent();
    }

    public static async Task<IResult> CloseBudgetList(
        [FromServices] CashPurseDbContext context, Guid budgetListId)
    {
        try
        {
            var budgetList = await FetchBudgetListById(budgetListId, context).ConfigureAwait(false);
            if (budgetList is null) return Results.NotFound("Budget list not found.");
            var reuslt = budgetList.CloseBudgetList();
            if (reuslt.Item2 == false) return Results.BadRequest(reuslt.Item1);
            await BudgetListDataService.UpdateBudgetList(context, budgetList).ConfigureAwait(false);
            return TypedResults.NoContent();
        }
        catch (Exception e)
        {
            return Results.Problem("There was a problem but the problem ain't yours!");
        }
    }
    
    public static async Task<PagedResult<BudgetListItemModel>> HandleGetBudgetListItems(
        [FromServices] CashPurseDbContext context, Guid budgetListId)
    {
        var results = await BudgetListDataService
            .BudgetListItems(context, budgetListId).ConfigureAwait(false);
        return results;
    }
    
    public static async Task<CursorPagedResult<List<BudgetListItemModel>>> HandleGetCursorPagedBudgetListItems(
        [FromServices] CashPurseDbContext context, Guid budgetListId, DateOnly cursor)
    {
        var results = await BudgetListDataService
            .CursorPagedBudgetListItems(context, budgetListId, cursor)
            .ConfigureAwait(false);
        return results;
    }

    public static Ok<BudgetListItemModel> HandleGetBudgetListItemById(
        [FromServices] CashPurseDbContext context, Guid budgetListId, Guid budgetListItemId)
    {
        var result = BudgetListDataService.BudgetListItemById(context, budgetListId, budgetListItemId);
        return TypedResults.Ok(result);
    }

    public static async Task<Created> CreateBudgetListItem([FromServices] CashPurseDbContext context,
       [FromRoute] Guid budgetListId, [FromBody] CreateBudgetListItemRequest inputModel)
    {
        var item = BudgetListItemMapper.MapCreateBudgetListItemRequest(inputModel);
        item.BudgetListId = budgetListId;
        await BudgetListDataService.AddNewBudgetListItem(context, item);
        return TypedResults.Created();
    }
    
    public static async Task<IResult> UpdateBudgetListItem([FromServices] CashPurseDbContext context, Guid budgetListId, Guid budgetListItemId, [FromBody] UpdateBudgetListItemRequest inputModel)
    {
        try
        {
            await BudgetListDataService.UpdateBudgetListItem(context, inputModel).ConfigureAwait(false);
            return TypedResults.NoContent();
        }
        catch (Exception)
        {
            return Results.Problem("There was a problem but the problem ain't yours!");
        }
    }

    internal static async Task<IResult> 
    MapAndCreateExpenseFromBudgetListItem([FromServices]CashPurseDbContext context, Guid budgetListId, Guid budgetListItemId, [FromBody] StrikeItemOffCurrentListRequest inputModel,
        CancellationToken token)
    {
        try
        {
            var budgetList = await context.BudgetLists
                .Include(b => b.BudgetItems)
                .Where(b => b.Id == budgetListId)
                .Where(b => b.BudgetItems.TrueForAll(x => x.Id == budgetListItemId))
                .FirstOrDefaultAsync(cancellationToken: token);
            var budgetListItem = budgetList?.BudgetItems.Single(x => x.Id == budgetListItemId);
            if (budgetList is null) return Results.BadRequest("Budget list or item not found.");
            // defaults to Nigerian Naira and other expense type.
            var expense = budgetList.MapBudgetListItemsToExpenses(ExpenseType.Other, Currency.NGN,
                budgetListItem!);
            //start transaction. Guard against failure. If there are failures, rollback everything.
            await context.Database.BeginTransactionAsync(token).ConfigureAwait(false);
            context.Expenses.Add(expense);
            await context.SaveChangesAsync(token).ConfigureAwait(false);
            budgetListItem?.UpdateBudgetListItemStatus(budgetListId);
            context.Entry(budgetListItem!).State = EntityState.Modified;
            await context.SaveChangesAsync(token).ConfigureAwait(false);
            await context.Database.CommitTransactionAsync(token).ConfigureAwait(false);
            return Results.Ok(new { expenseCreated = true, budgetListItemUpdated = true });
        }
        catch (Exception e)
        {
            await context.Database.RollbackTransactionAsync(token).ConfigureAwait(false);
            throw;
        }
    }
}

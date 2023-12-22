using System.Security.Claims;
using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.Data;
using CashPurse.Server.MapperConfiguration;
using CashPurse.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.EndpointHandlers;

public static class BudgetListEndpointHandler
{
    private static Task<ApplicationUser?> GetCurrentUser(ClaimsPrincipal principal, 
        UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name;
        return userManager.FindByNameAsync(userId!);
    }

    public static async Task<Ok<PagedResult<BudgetListModel>>> HandleGet([FromServices] CashPurseDbContext context)
    {
        var user = new ApplicationUser();
        var expense = await context.Expenses.AsNoTracking()
            // .Where(x => x.ExpenseOwnerId == user!.Id)
            .ToArrayAsync(CancellationToken.None);
        var alternative = await BudgetListDataService.UserBudgetLists(context)
            .ConfigureAwait(false);
        return TypedResults.Ok(alternative?.Items.Count == 0 ? alternative : alternative!);
    }
    
    public static async Task<Ok<CursorPagedResult<List<BudgetListModel>>>> HandleCursorPagedGet(
        [FromServices] CashPurseDbContext context, DateOnly cursor)
    {
        var user = new ApplicationUser();
        var expense = context.Expenses.AsNoTracking();//.First(x => x.ExpenseOwnerId == user!.Id);
        var alternative = await BudgetListDataService
            .CursorPagedUserBudgetLists(context, user!.Id, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(alternative?.Data.Count == 0 ? alternative : alternative!);
    }
    
    public static Ok<BudgetListModel> HandleGetById([FromServices] CashPurseDbContext context, Guid budgetListId)
    {
        var result = BudgetListDataService.BudgetListById(budgetListId, context);
        return TypedResults.Ok(result);
    }

    public static async Task<IResult> CreateBudgetList(
        [FromServices] CashPurseDbContext context, [FromBody]CreateBudgetListRequest inputModel)
    {
        try
        {
            var budgetList = BudgetListMapper.MapCreateBudgetList(inputModel);
            // var user = new ApplicationUser();
            // budgetList.OwnerId = inputModel.BudgetListOwnerId;
            await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
            return Results.Created($"budgetlists/{budgetList.Id}", budgetList);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
            entity.Description = inputModel.Description;
            entity.ExpenseId = Guid.Parse(inputModel.ExpenseId);
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
        
        // var user = new ApplicationUser();
        // budgetList.OwnerId = user!.Id; //clean up update budget list
        await BudgetListDataService.UpdateBudgetList(context, budgetList).ConfigureAwait(false);
        return TypedResults.NoContent();
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
    
    public static async Task<NoContent> UpdateBudgetListItem([FromServices] CashPurseDbContext context, Guid budgetListId,
        Guid budgetListItemId, [FromBody] UpdateBudgetListItemRequest inputModel)
    {
        await BudgetListDataService.UpdateBudgetListItem(context, inputModel).ConfigureAwait(false);
        return TypedResults.NoContent();
    }
}

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

    public static async Task<Ok<PagedResult<BudgetListModel>>> HandleGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        DateTime cursor)
    {
        try
        {
            var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
            var expense = context.Expenses.AsNoTracking().First(x => x.ExpenseOwnerId == user!.Id);
            var alternative = await BudgetListDataService.GetUserBudgetLists(context, expense.Id)
                .ConfigureAwait(false);
            if(alternative?.Items.Count == 0)
                return TypedResults.Ok(alternative);
            return TypedResults.Ok(alternative!);
        }
        catch (Exception )
        {
            throw;
        }
    }
    
    public static async Task<Ok<CursorPagedResult<List<BudgetListModel>>>> HandleCursorPagedGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        DateTime cursor)
    {
        try
        {
            var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
            var expense = context.Expenses.AsNoTracking().First(x => x.ExpenseOwnerId == user!.Id);
            var alternative = await BudgetListDataService.GetCursorPagedUserBudgetLists(context, expense.Id, cursor)
                .ConfigureAwait(false);
            if(alternative?.Data.Count == 0)
                return TypedResults.Ok(alternative);
            return TypedResults.Ok(alternative!);
        }
        catch (Exception )
        {
            // TODO: Add logging to better analyze cause of exceptions in api.
            throw;
        }
    }

    public static async Task<Created> CreateBudgetList(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody]CreateBudgetListRequest inputModel
    ) 
    {
        var budgetList = BudgetListMapper.MapCreateBudgetList(inputModel);
        await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
        return TypedResults.Created();
    }

    public static async Task<NoContent> UpdateBudgetList(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody]UpdateBudgetListRequest inputModel
    ) 
    {
        var budgetList = BudgetListMapper.MapUpdateBudgetList(inputModel);
        await BudgetListDataService.UpdateBudgetList(context, budgetList).ConfigureAwait(false);
        return TypedResults.NoContent();
    }
    
    public static async Task<PagedResult<BudgetListItemModel>> HandleGetBudgetListItems(
        [FromServices] CashPurseDbContext context, Guid budgetListId)
    {
        var results = await BudgetListDataService
            .GetBudgetListItems(context, budgetListId).ConfigureAwait(false);
        return results;
    }
    
    public static async Task<CursorPagedResult<List<BudgetListItemModel>>> HandleGetCursorPagedBudgetListItems(
        [FromServices] CashPurseDbContext context, Guid budgetListId, DateTime cursor)
    {
        var results = await BudgetListDataService
            .GetCursorPagedBudgetListItems(context, budgetListId, cursor)
            .ConfigureAwait(false);
        return results;
    }

    public static async Task<Created> CreateBudgetListItem([FromServices] CashPurseDbContext context,
        [FromBody] CreateBudgetListItemRequest inputModel)
    {
        var item = BudgetListItemMapper.MapCreateBudgetListItemRequest(inputModel);
        await BudgetListDataService.AddNewBudgetListItem(context, item);
        return TypedResults.Created();
    }
    
    public static async Task<NoContent> UpdateBudgetListItem([FromServices] CashPurseDbContext context,
        [FromBody] UpdateBudgetListItemRequest inputModel)
    {
        await BudgetListDataService.UpdateBudgetListItem(context, inputModel).ConfigureAwait(false);
        return TypedResults.NoContent();
    }
}

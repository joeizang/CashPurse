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
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager)
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var expense = await context.Expenses.AsNoTracking()
            .Where(x => x.ExpenseOwnerId == user!.Id)
            .ToArrayAsync(CancellationToken.None);
        var alternative = await BudgetListDataService.GetUserBudgetLists(context, user!.Id)
            .ConfigureAwait(false);
        return TypedResults.Ok(alternative?.Items.Count == 0 ? alternative : alternative!);
    }
    
    public static async Task<Ok<CursorPagedResult<List<BudgetListModel>>>> HandleCursorPagedGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        DateOnly cursor)
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var expense = context.Expenses.AsNoTracking().First(x => x.ExpenseOwnerId == user!.Id);
        var alternative = await BudgetListDataService
            .GetCursorPagedUserBudgetLists(context, user!.Id, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(alternative?.Data.Count == 0 ? alternative : alternative!);
    }

    public static async Task<Created> CreateBudgetList(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody]CreateBudgetListRequest inputModel
    ) 
    {
        try
        {
            var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
            var budgetList = BudgetListMapper.MapCreateBudgetList(inputModel);
            budgetList.OwnerId = user!.Id;
            await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
            return TypedResults.Created();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public static async Task<NoContent> UpdateBudgetList(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody]UpdateBudgetListRequest inputModel
    ) 
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var budgetList = BudgetListMapper.MapUpdateBudgetList(inputModel);
        budgetList.OwnerId = user!.Id; //clean up update budget list
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
        [FromServices] CashPurseDbContext context, Guid budgetListId, DateOnly cursor)
    {
        var results = await BudgetListDataService
            .GetCursorPagedBudgetListItems(context, budgetListId, cursor)
            .ConfigureAwait(false);
        return results;
    }

    public static async Task<Created> CreateBudgetListItem([FromServices] CashPurseDbContext context,
        Guid budgetListId, [FromBody] CreateBudgetListItemRequest inputModel)
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

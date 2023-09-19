using System.Security.Claims;
using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CashPurse.Server;

public static class BudgetListEndpointHandler
{
    private static Task<ApplicationUser?> GetCurrentUser(ClaimsPrincipal principal, UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name;
        return userManager.FindByNameAsync(userId!);
    }

    public static async Task<Ok<CursorPagedResult<List<BudgetListModel>>>> HandleGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [AsParameters] CursorPagedRequest cursor = default!)
    {
        try
        {
            var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
            var alternative = await BudgetListDataService.GetUserBudgetLists(context, userId: user!.Id, cursor:  cursor.Cursor)
                .ConfigureAwait(false);
            if(!alternative.Any())
                return TypedResults.Ok(new CursorPagedResult<List<BudgetListModel>>(DateTimeOffset.UtcNow, alternative));
            return TypedResults.Ok(new CursorPagedResult<List<BudgetListModel>>(alternative[^1].CreatedAt, alternative!));
        }
        catch (Exception )
        {
            throw;
        }
    }

    public static async Task<Created> CreateBudgetList(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [FromBody]CreateBudgetListModel inputModel
    ) 
    {
        var budgetList = BudgetListMapper.MapCreateBudgetList(inputModel);
        await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
        return TypedResults.Created();
    }
}

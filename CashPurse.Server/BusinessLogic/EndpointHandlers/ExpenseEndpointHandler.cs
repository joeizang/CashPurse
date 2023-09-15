using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Immutable;
using System.Security.Claims;

namespace CashPurse.Server.BusinessLogic.EndpointHandlers;

public static class ExpenseEndpointHandler
{
    public static async Task<Ok<CursorPagedResult<List<ExpenseIndexModel>>>> HandleGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name!;
        var user = await userManager.FindByNameAsync(userId).ConfigureAwait(false);
        //if(request.Value == DateTimeOffset.MinValue || request is null)
        //{
        //    var result = await ExpenseDataService.GetUserExpenses(context, user!.Id).ConfigureAwait(false);
        //    return TypedResults.Ok(new CursorPagedResult<IEnumerable<ExpenseIndexModel>>(result.Items[^1].ExpenseDate, result.Items));
        //}
        var request = DateTimeOffset.UtcNow;
        var alternative = await ExpenseDataService.GetCursorPagedUserExpenses(context, user!.Id, request).ConfigureAwait(false);
        return TypedResults.Ok(alternative!);
    }
}

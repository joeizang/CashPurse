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
        try
        {
            var userId = principal?.Identity?.Name!;
            var user = await userManager.FindByNameAsync(userId).ConfigureAwait(false);
            var cursor = DateTimeOffset.UtcNow;
            var alternative = await ExpenseDataService.GetCursorPagedUserExpenses(context, user!.Id, cursor).ConfigureAwait(false);
            return TypedResults.Ok(alternative!);
        }
        catch (Exception )
        {
            throw;
        }
    }

    public static async Task<Ok<ExpenseIndexModel>> HandleGetById(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        Guid expenseId)
    {
        try
        {
            var userId = principal?.Identity?.Name!;
            var user = await userManager.FindByNameAsync(userId).ConfigureAwait(false);
            var expense = await ExpenseDataService.GetExpenseById(context, user!.Id, expenseId).ConfigureAwait(false);
            return TypedResults.Ok(expense!);
        }
        catch (Exception )
        {
            throw;
        }
    }
}

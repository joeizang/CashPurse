using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.ExpensesApiModels;
using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CashPurse.Server.BusinessLogic.EndpointHandlers;

public static class ExpenseEndpointHandler
{
    public static async Task<Ok<CursorPagedResult<List<ExpenseIndexModel>>>> HandleGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        [FromRoute] CursorPagedRequest cursor = default!)
    {
        try
        {
            var userId = principal?.Identity?.Name!;
            var user = await userManager.FindByNameAsync(userId).ConfigureAwait(false);
            var alternative = await ExpenseDataService.GetCursorPagedUserExpenses(context, user!.Id, cursor.Cursor)
                .ConfigureAwait(false);
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
            var expense = ExpenseDataService.GetUserExpenseById(context, user?.Id!, expenseId);
            return TypedResults.Ok(expense!);
        }
        catch (Exception )
        {
            throw;
        }
    }

    public static async Task<Ok<CursorPagedResult<IEnumerable<ExpenseIndexModel>>>> HandleGetExpensesByCurrency(ClaimsPrincipal principal, [FromServices] CashPurseDbContext context,
        [FromServices] UserManager<ApplicationUser> userManager, [FromRoute] CursorPagedRequest cursor = default!) // use ExpenseDate for cursor in query
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var result = await ExpenseDataService
            .GetCurrencyUsedCursorPagedFilteredExpenses(context, user?.Id!, cursor.Cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }

    public static async Task<Ok<CursorPagedResult<IEnumerable<ExpenseIndexModel>>>> HandleGetExpenseByExpenseType(
        ClaimsPrincipal principal, [FromServices] UserManager<ApplicationUser> userManager,
        [FromServices] CashPurseDbContext context, [FromRoute] CursorPagedRequest cursor = default!)
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait (false);
        var result = await ExpenseDataService.GetCursorPagedTypeFilteredExpenses(context, user?.Id!, cursor.Cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }

    private static Task<ApplicationUser?> GetCurrentUser(ClaimsPrincipal principal, UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name;
        return userManager.FindByNameAsync(userId!);
    }
}

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
    internal static async Task<Ok<PagedResult<ExpenseIndexModel>>> HandleGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager)
    {
        try
        {
            var userId = principal?.Identity?.Name!;
            var user = await userManager.FindByEmailAsync(userId).ConfigureAwait(false);
            var alternative = await ExpenseDataService.GetUserExpenses(context, user!.Id)
                .ConfigureAwait(false);
            return TypedResults.Ok(alternative!);
        }
        catch (Exception )
        {
            throw;
        }
    }
    
    internal static async Task<Ok<CursorPagedResult<List<ExpenseIndexModel>>>> HandleCursorPagedGet(ClaimsPrincipal principal,
        [FromServices] CashPurseDbContext context, [FromServices] UserManager<ApplicationUser> userManager,
        DateOnly cursor)
    {

            var userId = principal?.Identity?.Name!;
            var user = await userManager.FindByEmailAsync(userId).ConfigureAwait(false);
            var alternative = await ExpenseDataService.CursorPagedUserExpenses(context,
                    user!.Id, cursor)
                .ConfigureAwait(false);
            return TypedResults.Ok(alternative!);

    }

    internal static async Task<Ok<ExpenseIndexModel>> HandleGetById(ClaimsPrincipal principal,
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

    internal static async Task<Ok<CursorPagedResult<IEnumerable<ExpenseIndexModel>>>> 
        HandleGetCursorPagedExpensesByCurrency(ClaimsPrincipal principal, [FromServices] CashPurseDbContext context,
        [FromServices] UserManager<ApplicationUser> userManager, DateOnly cursor) // use ExpenseDate for cursor in query
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var result = await ExpenseDataService
            .CurrencyUsedCursorPagedFilteredExpenses(context, user?.Id!, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }
    
    internal static async Task<Ok<PagedResult<ExpenseIndexModel>>> 
        HandleGetExpensesByCurrency(ClaimsPrincipal principal, [FromServices] CashPurseDbContext context,
            [FromServices] UserManager<ApplicationUser> userManager) // use ExpenseDate for cursor in query
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var result = await ExpenseDataService.GetCurrencyUsedFilteredExpenses(context, user?.Id!, 1)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }

    internal static async Task<Ok<CursorPagedResult<IEnumerable<ExpenseIndexModel>>>> HandleGetExpenseByExpenseType(
        ClaimsPrincipal principal, [FromServices] UserManager<ApplicationUser> userManager,
        [FromServices] CashPurseDbContext context, CursorPagedRequest cursor)
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait (false);
        var result = await ExpenseDataService.GetCursorPagedTypeFilteredExpenses(context, user?.Id!, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }

    private static Task<ApplicationUser?> GetCurrentUser(ClaimsPrincipal principal, UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name;
        return userManager.FindByNameAsync(userId!);
    }

    internal static async Task<Created> HandleCreateExpense(ClaimsPrincipal principal, [FromServices] CashPurseDbContext context,
        [FromServices] UserManager<ApplicationUser> userManager, [FromBody] CreateExpenseRequest request)
    {
        var user = await GetCurrentUser(principal, userManager).ConfigureAwait(false);
        var expense = ExpenseMapper.MapToCreateExpense(request);
        await ExpenseDataService.AddNewExpense(context, expense).ConfigureAwait(false);
        return TypedResults.Created();
    }

    internal static async Task<NoContent> HandleUpdateExpense(ClaimsPrincipal principal, [FromServices] CashPurseDbContext context,
        [FromServices] UserManager<ApplicationUser> userManager, [FromBody] UpdateExpenseRequest request,
        [FromRoute] Guid expenseId)
    {
        var target = await context.Expenses.FindAsync(expenseId).ConfigureAwait(false);
        await ExpenseDataService.UpdateExpense(context, target!, request).ConfigureAwait(false);
        return TypedResults.NoContent();
    }

    // internal static Task HandleDeleteExpense(HttpContext context)
    // {
    //     throw new NotImplementedException();
    // }
}

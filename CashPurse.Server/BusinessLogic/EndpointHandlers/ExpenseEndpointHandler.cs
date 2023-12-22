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
    internal static async Task<Ok<PagedResult<ExpenseIndexModel>>> HandleGet([FromServices] CashPurseDbContext context)
    {
        try
        {
            var user = new ApplicationUser();
            var alternative = await ExpenseDataService.UserExpenses(context)
                .ConfigureAwait(false);
            return TypedResults.Ok(alternative!);
        }
        catch (Exception )
        {
            throw;
        }
    }
    
    internal static async Task<Ok<CursorPagedResult<List<ExpenseIndexModel>>>> HandleCursorPagedGet(
        [FromServices] CashPurseDbContext context, DateOnly cursor)
    {
            var user = new ApplicationUser();
            var alternative = await ExpenseDataService.CursorPagedUserExpenses(context,
                    user!.Id, cursor)
                .ConfigureAwait(false);
            return TypedResults.Ok(alternative!);

    }

    internal static Ok<ExpenseIndexModel> HandleGetById(
        [FromServices] CashPurseDbContext context, Guid expenseId)
    {
        try
        {
            var user = new ApplicationUser();
            var expense = ExpenseDataService.UserExpenseById(context, user?.Id!, expenseId);
            return TypedResults.Ok(expense!);
        }
        catch (Exception )
        {
            throw;
        }
    }

    internal static async Task<Ok<CursorPagedResult<IEnumerable<ExpenseIndexModel>>>> 
        HandleGetCursorPagedExpensesByCurrency([FromServices] CashPurseDbContext context, DateOnly cursor)
    {
        var user = new ApplicationUser();
        var result = await ExpenseDataService
            .CurrencyUsedCursorPagedFilteredExpenses(context, user?.Id!, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }
    
    internal static async Task<Ok<PagedResult<ExpenseIndexModel>>> 
        HandleGetExpensesByCurrency(ClaimsPrincipal principal, [FromServices] CashPurseDbContext context)
    {
        var user = new ApplicationUser();
        var result = await ExpenseDataService.CurrencyUsedFilteredExpenses(context, user?.Id!, 1)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }

    internal static async Task<Ok<CursorPagedResult<IEnumerable<ExpenseIndexModel>>>> HandleGetExpenseByExpenseType(
        [FromServices] CashPurseDbContext context, CursorPagedRequest cursor)
    {
        var user = new ApplicationUser();
        var result = await ExpenseDataService.CursorPagedTypeFilteredExpenses(context, user?.Id!, cursor)
            .ConfigureAwait(false);
        return TypedResults.Ok(result);
    }

    private static Task<ApplicationUser?> GetCurrentUser(ClaimsPrincipal principal, UserManager<ApplicationUser> userManager)
    {
        var userId = principal?.Identity?.Name;
        return userManager.FindByNameAsync(userId!);
    }

    internal static async Task<Created> HandleCreateExpense(
        [FromServices] CashPurseDbContext context, [FromBody] CreateExpenseRequest request)
    {
        var user = new ApplicationUser();
        var expense = ExpenseMapper.MapToCreateExpense(request);
        await ExpenseDataService.AddNewExpense(context, expense).ConfigureAwait(false);
        return TypedResults.Created();
    }

    internal static async Task<NoContent> HandleUpdateExpense( 
        [FromServices] CashPurseDbContext context, [FromBody] UpdateExpenseRequest request,
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

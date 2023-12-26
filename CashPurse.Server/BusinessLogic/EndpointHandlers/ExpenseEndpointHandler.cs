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
        [FromServices] CashPurseDbContext context, [FromBody] CreateExpenseRequest request,
        CancellationToken token = default)
    {
        var user = new ApplicationUser();
        // var expense = ExpenseMapper.MapToExpense(request);
        var expense = new Expense
        {
            Name = request.Name,
            Description = request.Description,
            Amount = request.Amount,
            ExpenseDate = request.ExpenseDate,
            ExpenseType = request.ExpenseType,
            CurrencyUsed = request.CurrencyUsed,
            Notes = request.Notes
        };

        await ExpenseDataService.AddNewExpense(context, expense, token).ConfigureAwait(false);
        return TypedResults.Created();
    }

    private static Func<Guid, CashPurseDbContext, Task<Expense?>> FetchExpenseById = async (id, context) => 
        await context.Expenses.FindAsync(id).ConfigureAwait(false);
    internal static async Task<NoContent> HandleUpdateExpense( 
        [FromServices] CashPurseDbContext context, [FromBody] UpdateExpenseRequest request,
        [FromRoute] Guid expenseId, CancellationToken token = default)
    {
        try
        {
            var target = await FetchExpenseById(expenseId, context).ConfigureAwait(false);
            await ExpenseDataService.UpdateExpense(context, target!, request, token).ConfigureAwait(false);
            return TypedResults.NoContent();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    // internal static Task HandleDeleteExpense(HttpContext context)
    // {
    //     throw new NotImplementedException();
    // }
}

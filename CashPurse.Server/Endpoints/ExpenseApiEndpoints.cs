using CashPurse.Server.BusinessLogic.EndpointHandlers;
using CashPurse.Server.CompiledEFQueries;

namespace CashPurse.Server.Endpoints;

public static class ExpenseApiEndpoints
{
    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expenseGroup = app.MapGroup("/api/expenses");
        var expenseGroupWithIds = expenseGroup.MapGroup("/{expenseId:guid}");

        expenseGroup.MapGet("", ExpenseEndpointHandler.HandleGet).CacheOutput("CacheDataPage");
        expenseGroup.MapGet("/bycurrency", ExpenseEndpointHandler.HandleGetExpensesByCurrency).CacheOutput("CacheDataPage");
        expenseGroup.MapGet("/bytype", ExpenseEndpointHandler.HandleGetExpenseByExpenseType).CacheOutput("CacheDataPage");
        expenseGroupWithIds.MapGet("", ExpenseEndpointHandler.HandleGetById).CacheOutput("CacheDataPage");

        expenseGroup.MapPost("", ExpenseEndpointHandler.HandleCreateExpense);
        expenseGroupWithIds.MapPut("", ExpenseEndpointHandler.HandleUpdateExpense);
        expenseGroupWithIds.MapDelete("", ExpenseEndpointHandler.HandleDeleteExpense);

        return expenseGroup;
    }
}

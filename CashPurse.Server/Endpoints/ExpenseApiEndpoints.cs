using CashPurse.Server.BusinessLogic.EndpointHandlers;
using CashPurse.Server.CompiledEFQueries;

namespace CashPurse.Server.Endpoints;

public static class ExpenseApiEndpoints
{
    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expenseGroup = app.MapGroup("/api/expenses");
        var expenseGroupWithIds = expenseGroup.MapGroup("/{expenseId:guid}");

        expenseGroup.MapGet("", ExpenseEndpointHandler.HandleGet);

        return expenseGroup;
    }
}

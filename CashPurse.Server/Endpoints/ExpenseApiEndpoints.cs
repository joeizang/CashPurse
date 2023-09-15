using Microsoft.AspNetCore.Routing;

namespace CashPurse.Server.Endpoints;

public static class ExpenseApiEndpoints
{
    public static void MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expensesEndpoints = app.MapGroup("/api/expenses");
        
    }
}

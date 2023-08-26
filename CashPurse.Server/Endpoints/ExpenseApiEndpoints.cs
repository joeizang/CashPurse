namespace CashPurse.Server.Endpoints;

public static class ExpenseApiEndpoints
{
    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expenseGroup = app.MapGroup("/api/expenses");
        var expenseGroupWithIds = expenseGroup.MapGroup("/{expenseId:guid}");

        return expenseGroup;
    }
}

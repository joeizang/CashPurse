using CashPurse.Server.BusinessLogic.EndpointHandlers;

namespace CashPurse.Server;

public static class BudgetListApiEndpoints
{
    public static RouteGroupBuilder MapBudgetListEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetListGroup = app.MapGroup("/api/budgetlists");
        var budgetListGroupWithIds = budgetListGroup.MapGroup("/{budgetListId:guid}");

        budgetListGroup.MapGet("", BudgetListEndpointHandler.HandleGet);

        return budgetListGroup;
    }
}

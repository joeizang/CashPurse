using CashPurse.Server.BusinessLogic.EndpointFilters;
using CashPurse.Server.BusinessLogic.EndpointHandlers;

namespace CashPurse.Server;

public static class BudgetListApiEndpoints
{
    public static RouteGroupBuilder MapBudgetListEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetListGroup = app.MapGroup("/api/budgetlists");
        var budgetListGroupWithIds = budgetListGroup.MapGroup("/{budgetListId:guid}");

        budgetListGroup.MapGet("", BudgetListEndpointHandler.HandleGet);

        budgetListGroup.MapGet("/cursor", BudgetListEndpointHandler.HandleCursorPagedGet);

        budgetListGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetList)
            .AddEndpointFilter<CreateBudgetListFilter>();

        budgetListGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetList);

        return budgetListGroup;
    }
}

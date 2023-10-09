using CashPurse.Server.BusinessLogic.EndpointFilters;
using CashPurse.Server.BusinessLogic.EndpointHandlers;

namespace CashPurse.Server;

public static class BudgetListApiEndpoints
{
    public static RouteGroupBuilder MapBudgetListEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetListGroup = app.MapGroup("/api/budgetlists").RequireAuthorization();
        var budgetListGroupWithIds = budgetListGroup.MapGroup("/{budgetListId:guid}");
        var budgetListItemGroup = budgetListGroupWithIds.MapGroup("/budgetListItem");

        budgetListGroup.MapGet("", BudgetListEndpointHandler.HandleGet);

        budgetListGroup.MapGet("/cursor", BudgetListEndpointHandler.HandleCursorPagedGet);

        budgetListGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetList)
            .AddEndpointFilter<CreateBudgetListFilter>();
        
        budgetListGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetList)
            .AddEndpointFilter<UpdateBudgetListFilter>();
        
        budgetListItemGroup.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItems);
        budgetListItemGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetListItem)
            .AddEndpointFilter<CreateBudgetListItemFilter>();

        budgetListItemGroup.MapPut("/{id:Guid}", BudgetListEndpointHandler.UpdateBudgetListItem)
            .AddEndpointFilter<UpdateBudgetListItemFilter>();

        return budgetListGroup;
    }
}

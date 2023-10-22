using CashPurse.Server.BusinessLogic.EndpointFilters;
using CashPurse.Server.BusinessLogic.EndpointHandlers;
using CashPurse.Server.Models;

namespace CashPurse.Server.Endpoints;

public static class BudgetListApiEndpoints
{
    public static RouteGroupBuilder MapBudgetListEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetListGroup = app.MapGroup("/api/budgetlists").RequireAuthorization();
        var budgetListGroupWithIds = budgetListGroup.MapGroup("/{budgetListId:guid}");
        var budgetListItemGroup = budgetListGroupWithIds.MapGroup("/budgetListItem");
        var budgetListItemGroupWithIds = budgetListItemGroup.MapGroup("/{budgetListItemId:guid}");

        budgetListGroup.MapGet("", BudgetListEndpointHandler.HandleGet)
        .CacheOutput("CacheDataPage");

        budgetListGroup.MapGet("/cursor", BudgetListEndpointHandler.HandleCursorPagedGet);

        budgetListGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetById)
            .AddEndpointFilter<GetBudgetListByIdFilter>()
            .CacheOutput("CacheDataPage");
        
        budgetListGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetList)
            .AddEndpointFilter<CreateBudgetListFilter>();
        
        budgetListGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetList)
            .AddEndpointFilter<UpdateBudgetListFilter>();

        budgetListItemGroup.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItems)
            .AddEndpointFilter<FetchBudgetListItemsFilter>()
            .CacheOutput("CacheDataPage");
        budgetListItemGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItemById)
            .AddEndpointFilter<GetBudgetListItemByIdFilter>()
            .CacheOutput("CacheDataPage");
        budgetListItemGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetListItem)
            .AddEndpointFilter<CreateBudgetListItemFilter>();

        budgetListItemGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetListItem)
            .AddEndpointFilter<UpdateBudgetListItemFilter>()
            .CacheOutput("CacheDataPage");

        return budgetListGroup;
    }
}

using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.BusinessLogic.EndpointFilters;
using CashPurse.Server.BusinessLogic.EndpointHandlers;
using CashPurse.Server.Data;
using CashPurse.Server.Models;

namespace CashPurse.Server.Endpoints;

public static class BudgetListApiEndpoints
{
    public static RouteGroupBuilder MapBudgetListEndpoints(this IEndpointRouteBuilder app)
    {
        var budgetListGroup = app.MapGroup("/api/budgetlists");//.RequireAuthorization();
        var budgetListGroupWithIds = budgetListGroup.MapGroup("/{budgetListId:guid}");
        var budgetListItemGroup = budgetListGroupWithIds.MapGroup("/budgetlistitems");
        var budgetListItemGroupWithIds = budgetListItemGroup.MapGroup("/{budgetListItemId:guid}");

        //==> QUERIES
        budgetListGroup.MapGet("", BudgetListEndpointHandler.HandleGet)
            .CacheOutput("CacheDataPage");
        budgetListGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetById)
            .CacheOutput("CacheDataPage");
        budgetListGroup.MapGet("/paged", BudgetListEndpointHandler.HandleCursorPagedGet)
            .CacheOutput("CacheDataPage");
        //==> COMMANDS
        budgetListGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetList);
        budgetListGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetList);
        //     .RequireCors("AllowAll");
        //
        
        //     // .AddEndpointFilter<GetBudgetListByIdFilter>()
        //     .RequireCors("AllowAll");
        //
        //     // .AddEndpointFilter<CreateBudgetListFilter>()
        //     .RequireCors("AllowAll");
        //
        //     // .AddEndpointFilter<UpdateBudgetListFilter>()
        //     .RequireCors("AllowAll");
        //
        // ==> BUDGETLISTITEMS QUERIES
        budgetListItemGroup.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItems)
            .CacheOutput("CacheDataPage");
        budgetListItemGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItemById)
            .CacheOutput("CacheDataPage");
        //     // .AddEndpointFilter<FetchBudgetListItemsFilter>()
        //     .RequireCors("AllowAll");
        //     // .AddEndpointFilter<GetBudgetListItemByIdFilter>()
        //     .RequireCors("AllowAll");

        // ==> BUDGETLISTITEMS COMMANDS
        budgetListItemGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetListItem);
        budgetListItemGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetListItem);
        //     // .AddEndpointFilter<CreateBudgetListItemFilter>()
        //     .RequireCors("AllowAll");
        //
        //     // .AddEndpointFilter<UpdateBudgetListItemFilter>()
        //     .RequireCors("AllowAll");

        return budgetListGroup;
    }
}

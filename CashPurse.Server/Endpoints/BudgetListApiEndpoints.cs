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
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        budgetListGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetById)
            .AddEndpointFilter<GetBudgetListByIdFilter>()
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        budgetListGroup.MapGet("/paged", BudgetListEndpointHandler.HandleCursorPagedGet)
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        //==> COMMANDS
        budgetListGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetList);
        budgetListGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetList);
        budgetListGroupWithIds.MapPut("/closeList", BudgetListEndpointHandler.CloseBudgetList);
        
        // ==> BUDGETLISTITEMS QUERIES
        budgetListItemGroup.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItems)
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        budgetListItemGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItemById)
            .AddEndpointFilter<GetBudgetListItemByIdFilter>()
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");

        // ==> BUDGETLISTITEMS COMMANDS
        budgetListItemGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetListItem)
            .AddEndpointFilter<CreateBudgetListItemFilter>()
            .RequireCors("AllowAll");
        budgetListItemGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetListItem)
            .AddEndpointFilter<UpdateBudgetListItemFilter>()
            .RequireCors("AllowAll");
        budgetListItemGroupWithIds.MapPut("/strikeitemoff", BudgetListEndpointHandler.MapAndCreateExpenseFromBudgetListItem)
            // .AddEndpointFilter<StrikeItemOffCurrentListFilter>()
            .RequireCors("AllowAll");
        return budgetListGroup;
    }
}

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

        budgetListGroup.MapGet("", async (CashPurseDbContext context) =>
        {
            var result = await BudgetListDataService.GetUserBudgetLists(context).ConfigureAwait(false);
            return Results.Ok(result);
        });
        // var budgetListItemGroup = budgetListGroupWithIds.MapGroup("/budgetListItem");
        // var budgetListItemGroupWithIds = budgetListItemGroup.MapGroup("/{budgetListItemId:guid}");
        //
        // // budgetListGroup.MapGet("", BudgetListEndpointHandler.HandleGet)
        // //     .AllowAnonymous()
        // // .CacheOutput("CacheDataPage")
        // //     .RequireCors("AllowAll");
        //
        // budgetListGroup.MapGet("/cursor", BudgetListEndpointHandler.HandleCursorPagedGet)
        //     .CacheOutput("CacheDataPage")
        //     .RequireCors("AllowAll");
        //
        // budgetListGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetById)
        //     // .AddEndpointFilter<GetBudgetListByIdFilter>()
        //     .CacheOutput("CacheDataPage")
        //     .RequireCors("AllowAll");
        //
        // budgetListGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetList)
        //     // .AddEndpointFilter<CreateBudgetListFilter>()
        //     .RequireCors("AllowAll");
        //
        // budgetListGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetList)
        //     // .AddEndpointFilter<UpdateBudgetListFilter>()
        //     .RequireCors("AllowAll");
        //
        // budgetListItemGroup.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItems)
        //     // .AddEndpointFilter<FetchBudgetListItemsFilter>()
        //     .CacheOutput("CacheDataPage")
        //     .RequireCors("AllowAll");
        // budgetListItemGroupWithIds.MapGet("", BudgetListEndpointHandler.HandleGetBudgetListItemById)
        //     // .AddEndpointFilter<GetBudgetListItemByIdFilter>()
        //     .CacheOutput("CacheDataPage")
        //     .RequireCors("AllowAll");
        // budgetListItemGroup.MapPost("", BudgetListEndpointHandler.CreateBudgetListItem)
        //     // .AddEndpointFilter<CreateBudgetListItemFilter>()
        //     .RequireCors("AllowAll");
        //
        // budgetListItemGroupWithIds.MapPut("", BudgetListEndpointHandler.UpdateBudgetListItem)
        //     // .AddEndpointFilter<UpdateBudgetListItemFilter>()
        //     .RequireCors("AllowAll");

        return budgetListGroup;
    }
}

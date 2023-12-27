using CashPurse.Server.ApiModels;
using CashPurse.Server.BusinessLogic.EndpointFilters;
using CashPurse.Server.BusinessLogic.EndpointHandlers;

namespace CashPurse.Server.Endpoints;

public static class ExpenseApiEndpoints
{
    private static readonly string[] value = ["There is no expense with this ID!"];

    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expenseGroup = app.MapGroup("/api/expenses");//.RequireAuthorization();
        var expenseGroupWithIds = expenseGroup.MapGroup("/{expenseId:guid}");

        expenseGroup.MapGet("", ExpenseEndpointHandler.HandleGet)
            // .AllowAnonymous()
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        expenseGroup.MapGet("/cursor-paged", ExpenseEndpointHandler.HandleCursorPagedGet).RequireCors("AllowAll");
        expenseGroup.MapGet("/bycurrency", ExpenseEndpointHandler.HandleGetExpensesByCurrency)
            .CacheOutput("CacheDataPage")
            // .AddEndpointFilter<ExpenseGetFilter>()
            .RequireCors("AllowAll");
        expenseGroup.MapGet("/cursor-paged-by-currency", ExpenseEndpointHandler
                .HandleGetCursorPagedExpensesByCurrency)
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        expenseGroup.MapGet("/bytype", ExpenseEndpointHandler.HandleGetExpenseByExpenseType)
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");
        expenseGroupWithIds.MapGet("", ExpenseEndpointHandler.HandleGetById)
            .CacheOutput("CacheDataPage")
            .RequireCors("AllowAll");

        expenseGroup.MapPost("", ExpenseEndpointHandler.HandleCreateExpense)
            // .AddEndpointFilter<ExpenseCreateFilter>()
            .RequireCors("AllowAll");
        expenseGroupWithIds.MapPut("", ExpenseEndpointHandler.HandleUpdateExpense)
            // .AddEndpointFilter(async(context, next) =>
            // {
            //     var body = context.GetArgument<UpdateExpenseRequest>(3);
            //     var id = context.GetArgument<Guid>(4);
            //     if (id != body.ExpenseId)
            //     {
            //         var errors = new Dictionary<string, string[]>
            //         {
            //             { "Error", value }
            //         };
            //         return Results.Problem("Expense IDs don't match!");
            //     }
            //
            //     return await next(context);
            // })
            // .AddEndpointFilter<ExpenseUpdateFilter>()
            .RequireCors("AllowAll");
        // expenseGroupWithIds.MapDelete("", ExpenseEndpointHandler.HandleDeleteExpense);

        return expenseGroup;
    }
}

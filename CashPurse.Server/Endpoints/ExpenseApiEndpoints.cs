using CashPurse.Server.ApiModels;
using CashPurse.Server.BusinessLogic.EndpointFilters;
using CashPurse.Server.BusinessLogic.EndpointHandlers;

namespace CashPurse.Server.Endpoints;

public static class ExpenseApiEndpoints
{
    private static readonly string[] value = ["There is no expense with this ID!"];

    public static RouteGroupBuilder MapExpenseEndpoints(this IEndpointRouteBuilder app)
    {
        var expenseGroup = app.MapGroup("/api/expenses").RequireAuthorization();
        var expenseGroupWithIds = expenseGroup.MapGroup("/{expenseId:guid}");

        expenseGroup.MapGet("", ExpenseEndpointHandler.HandleGet)
            .CacheOutput("CacheDataPage");
        expenseGroup.MapGet("/cursor-paged", ExpenseEndpointHandler.HandleCursorPagedGet);
        expenseGroup.MapGet("/bycurrency", ExpenseEndpointHandler.HandleGetExpensesByCurrency)
            .CacheOutput("CacheDataPage")
            .AddEndpointFilter<ExpenseGetFilter>();
        expenseGroup.MapGet("/cursor-paged-by-currency", ExpenseEndpointHandler
                .HandleGetCursorPagedExpensesByCurrency)
            .CacheOutput("CacheDataPage");
        expenseGroup.MapGet("/bytype", ExpenseEndpointHandler.HandleGetExpenseByExpenseType)
            .CacheOutput("CacheDataPage");
        expenseGroupWithIds.MapGet("", ExpenseEndpointHandler.HandleGetById)
            .CacheOutput("CacheDataPage");

        expenseGroup.MapPost("", ExpenseEndpointHandler.HandleCreateExpense)
            .AddEndpointFilter<ExpenseCreateFilter>();
        expenseGroupWithIds.MapPut("", ExpenseEndpointHandler.HandleUpdateExpense)
            .AddEndpointFilter(async(context, next) =>
            {
                var body = context.GetArgument<UpdateExpenseRequest>(3);
                var id = context.GetArgument<Guid>(4);
                if (id != body.ExpenseId)
                {
                    var errors = new Dictionary<string, string[]>
                    {
                        { "Error", value }
                    };
                    return Results.Problem("Expense IDs don't match!");
                }

                return await next(context);
            })
            .AddEndpointFilter<ExpenseUpdateFilter>();
        // expenseGroupWithIds.MapDelete("", ExpenseEndpointHandler.HandleDeleteExpense);

        return expenseGroup;
    }
}

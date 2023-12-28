
namespace CashPurse.Server;

public class GetBudgetListItemByIdFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var budgetListId = context.GetArgument<Guid>(1);
        var budgetListItemId = context.GetArgument<Guid>(2);
        return budgetListItemId == Guid.Empty || budgetListId == Guid.Empty
         ? Results.BadRequest("Ids cannot be in an invalid state!") : await next(context);
    }
}


namespace CashPurse.Server;

public class GetBudgetListItemByIdFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var budgetListItemId = context.GetArgument<Guid>(2);
        return budgetListItemId == Guid.Empty ? Results.Problem("Guid cannot be in an invalid state!") : await next(context);
    }
}

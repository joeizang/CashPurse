namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class ExpenseGetFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        return await next(context);
    }
}

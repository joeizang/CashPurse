using CashPurse.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class FetchBudgetListItemsFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<Guid>(1);
        var db = context.GetArgument<CashPurseDbContext>(0);
        return id == Guid.Empty ? Results.BadRequest("Invalid BudgetList Id") 
            : await next(context);
    }
}

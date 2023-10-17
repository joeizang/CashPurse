using CashPurse.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class FetchBudgetListItemsFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<Guid>(1);
        var db = context.GetArgument<CashPurseDbContext>(0);
        var result = await db.BudgetLists.CountAsync(x => x.Id == id).ConfigureAwait(false);
        return result == 0 ? Results.Problem("Invalid BudgetList Id", statusCode: 400) 
            : await next(context);
    }
}

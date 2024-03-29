﻿
using CashPurse.Server.Data;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class GetBudgetListByIdFilter : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var id = context.GetArgument<Guid>(1);
        var db = context.GetArgument<CashPurseDbContext>(0);
        if(id == Guid.Empty)
            return Results.NotFound("Budget list not found.");
        return await next(context).ConfigureAwait(false);
    }
}

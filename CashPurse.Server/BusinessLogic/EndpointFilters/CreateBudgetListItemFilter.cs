using CashPurse.Server.ApiModels.BudgetListApiModels;
using CashPurse.Server.Data;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class CreateBudgetListItemFilter(IValidator<CreateBudgetListItemRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var items = context.GetArgument<CreateBudgetListItemRequest>(2);
        var result = await validator.ValidateAsync(items).ConfigureAwait(false);
        return result.IsValid == false ? Results.ValidationProblem(result.ToDictionary()) : await next(context);
    }
}


public class UpdateBudgetListItemFilter(IValidator<UpdateBudgetListItemRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var db = context.GetArgument<CashPurseDbContext>(0);
        var budgetListItemId = context.GetArgument<Guid>(2);
        var budgetListItem = await db.BudgetListItems.FindAsync(budgetListItemId).ConfigureAwait(false);
        if(budgetListItem is null) return Results.NotFound("The budget list item was not found.");
        var item = context.GetArgument<UpdateBudgetListItemRequest>(3);
        var result = await validator.ValidateAsync(item).ConfigureAwait(false);
        return result.IsValid == false ? Results.ValidationProblem(result.ToDictionary()) : await next(context);
    }
}

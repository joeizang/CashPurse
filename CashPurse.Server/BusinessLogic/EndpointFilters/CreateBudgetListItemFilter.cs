using CashPurse.Server.ApiModels.BudgetListApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class CreateBudgetListItemFilter(IValidator<CreateBudgetListItemRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var items = context.GetArgument<CreateBudgetListItemRequest>(3);
        var result = await validator.ValidateAsync(items).ConfigureAwait(false);
        return result.IsValid == false ? Results.ValidationProblem(result.ToDictionary()) : await next(context);
    }
}


public class UpdateBudgetListItemFilter(IValidator<UpdateBudgetListItemRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var item = context.GetArgument<UpdateBudgetListItemRequest>(3);
        var result = await validator.ValidateAsync(item).ConfigureAwait(false);
        return result.IsValid == false ? Results.ValidationProblem(result.ToDictionary()) : await next(context);
    }
}

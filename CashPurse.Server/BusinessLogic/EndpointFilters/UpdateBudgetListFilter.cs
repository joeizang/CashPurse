using CashPurse.Server.ApiModels;
using CashPurse.Server.ApiModels.BudgetListApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class UpdateBudgetListFilter(IValidator<UpdateBudgetListRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var budget = context.GetArgument<UpdateBudgetListRequest>(3);
        var result = await validator.ValidateAsync(budget).ConfigureAwait(false);
        return result.IsValid == false ? Results.ValidationProblem(result.ToDictionary()) : await next(context);
    }
}

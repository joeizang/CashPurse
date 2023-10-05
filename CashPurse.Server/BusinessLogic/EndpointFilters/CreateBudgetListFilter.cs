using CashPurse.Server.ApiModels.BudgetListApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class CreateBudgetListFilter(IValidator<CreateBudgetListRequest> validator) : IEndpointFilter
{
    
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var inputModel = context.GetArgument<CreateBudgetListRequest>(3);
        var result = await validator.ValidateAsync(inputModel).ConfigureAwait(false);
        return result.IsValid == false ? Results.ValidationProblem(result.ToDictionary()) : await next(context);
    }
}

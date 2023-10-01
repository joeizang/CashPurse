using CashPurse.Server.ApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class ExpenseUpdateFilter(IValidator<UpdateExpenseRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var request = context.GetArgument<UpdateExpenseRequest>(3);
        var validationResult = await validator.ValidateAsync(request);
        return validationResult.IsValid ? await next(context)
            : Results.ValidationProblem(validationResult.ToDictionary());
    }
}

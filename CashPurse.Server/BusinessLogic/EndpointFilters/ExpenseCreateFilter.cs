using CashPurse.Server.ApiModels;
using CashPurse.Server.Models;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.EndpointFilters;

public class ExpenseCreateFilter(IValidator<CreateExpenseRequest> validator) : IEndpointFilter
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var input = context.GetArgument<CreateExpenseRequest>(3);
        var validationResult = await validator.ValidateAsync(input);
        return validationResult.IsValid == false
            ? Results.ValidationProblem(validationResult.ToDictionary())
            : await next(context);
    }
}

using CashPurse.Server.ApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.Validators;

public class ExpenseUpdateValidator : AbstractValidator<UpdateExpenseRequest>
{
    public ExpenseUpdateValidator()
    {
        RuleFor(e => e.ExpenseId)
            .NotEqual(Guid.Empty);
        RuleFor(e => e.Name)
            .MaximumLength(255)
            .NotEmpty()
            .NotEqual("");
        RuleFor(e => e.ExpenseDate)
            .NotEqual(DateOnly.MinValue)
            .NotEqual(DateOnly.MaxValue)
            .NotEqual(DateOnly.FromDateTime(DateTime.Now.AddDays(1)));
        RuleFor(e => e.Amount)
            .NotEqual(decimal.MinValue)
            .NotEqual(decimal.MaxValue)
            .NotEqual(decimal.Zero);
        RuleFor(e => e.ExpenseOwnerId)
            .NotEmpty()
            .WithMessage("Every expense must have an owner.");
    }
}

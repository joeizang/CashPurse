using CashPurse.Server.ApiModels.BudgetListApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.Validators;

public class BudgetListItemValidator : AbstractValidator<CreateBudgetListItemRequest>
{
    public BudgetListItemValidator()
    {
        RuleFor(b => b.ListName)
            .NotEmpty()
            .WithName(nameof(CreateBudgetListItemRequest.ListName))
            .WithMessage(b => $"'{b.ListName}' cannot be an empty string!")
            .MaximumLength(150);
        RuleFor(b => b.Description)
            .MaximumLength(300);
        RuleFor(b => b.UnitPrice)
            .GreaterThanOrEqualTo(0m);
        RuleFor(b => b.Quantity)
            .GreaterThanOrEqualTo(1);
        RuleFor(b => b.Price)
            .GreaterThanOrEqualTo(0m);
    }
}

public class UpdateBudgetListItemValidator : AbstractValidator<UpdateBudgetListItemRequest>
{
    public UpdateBudgetListItemValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty()
            .WithName(nameof(UpdateBudgetListItemRequest.Name))
            .WithMessage(b => $"'{b.Name}' cannot be an empty string!")
            .MaximumLength(150);
        RuleFor(b => b.Description)
            .MaximumLength(300);
        RuleFor(b => b.UnitPrice)
            .GreaterThanOrEqualTo(0m);
        RuleFor(b => b.Quantity)
            .GreaterThanOrEqualTo(1);
        RuleFor(b => b.Price)
            .GreaterThanOrEqualTo(0m);
    }
}

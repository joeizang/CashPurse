using CashPurse.Server.ApiModels.BudgetListApiModels;
using FluentValidation;

namespace CashPurse.Server.BusinessLogic.Validators;

public class BudgetListCreateValidator : AbstractValidator<CreateBudgetListRequest>
{
    public BudgetListCreateValidator()
    {
        RuleFor(b => b.Name)
            .NotEmpty()
            .WithName(nameof(CreateBudgetListRequest.Name))
            .WithMessage(b => $"'{b.Name}' cannot be empty!")
            .MaximumLength(300)
            .WithMessage("Name is too long and won't be accepted");
        RuleFor(b => b.Description)
            .MaximumLength(500);
        RuleFor(b => b.BudgetListOwnerId)
            .NotEmpty()
            .WithName(nameof(CreateBudgetListRequest.BudgetListOwnerId))
            .WithMessage(b => $"'{b.BudgetListOwnerId}' cannot be empty!");
    }
}

public class BudgetListUpdateValidator : AbstractValidator<UpdateBudgetListRequest>
{
    public BudgetListUpdateValidator()
    {
        RuleFor(b => b.ListName)
            .NotEmpty()
            .WithName(nameof(UpdateBudgetListRequest.ListName))
            .WithMessage(b => $"'{b.ListName}' cannot be empty!")
            .MaximumLength(300)
            .WithMessage("Name is too long and won't be accepted");
        RuleFor(b => b.Description)
            .MaximumLength(500);
    }
} 

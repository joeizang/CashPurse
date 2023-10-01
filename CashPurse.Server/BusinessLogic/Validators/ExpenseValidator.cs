using System.Data;
using CashPurse.Server.ApiModels;
using CashPurse.Server.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.Validators;

public class ExpenseValidator : AbstractValidator<CreateExpenseRequest>
{
    private readonly UserManager<ApplicationUser> _userManager;

    public ExpenseValidator(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
        RuleFor(e => e.Name)
            .MaximumLength(255)
            .NotEmpty()
            .NotEqual("");
        RuleFor(e => e.ExpenseDate)
            .NotEqual(DateTime.MinValue)
            .NotEqual(DateTime.MaxValue)
            .NotEqual(DateTime.Now.AddDays(-32));
        RuleFor(e => e.Amount)
            .NotEqual(decimal.MinValue)
            .NotEqual(decimal.MaxValue)
            .NotEqual(decimal.Zero);
        RuleFor(e => e.ExpenseOwnerId)
            .NotEmpty()
            .WithMessage("Every expense must have an owner.");
    }
}

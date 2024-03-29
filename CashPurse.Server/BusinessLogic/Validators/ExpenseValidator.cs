using System.Data;
using CashPurse.Server.ApiModels;
using CashPurse.Server.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.BusinessLogic.Validators;

public class ExpenseValidator : AbstractValidator<CreateExpenseRequest>
{

    public ExpenseValidator()
    {
        RuleFor(e => e.Name)
            .MaximumLength(255)
            .NotEmpty()
            .NotEqual("");
        RuleFor(e => e.ExpenseDate)
            .NotEqual(DateOnly.MinValue)
            .NotEqual(DateOnly.MaxValue)
            .NotEqual(DateOnly.FromDateTime(DateTime.Now.AddDays(-32)));
        RuleFor(e => e.Amount)
            .NotEqual(decimal.MinValue)
            .NotEqual(decimal.MaxValue)
            .NotEqual(decimal.Zero);
        // RuleFor(e => e.ExpenseOwnerId)
        //     .NotEmpty()
        //     .WithMessage("Every expense must have an owner.");
    }
}

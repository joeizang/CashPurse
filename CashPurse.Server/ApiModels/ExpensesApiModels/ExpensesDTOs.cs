using CashPurse.Server.Models;
using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace CashPurse.Server.ApiModels.ExpensesApiModels;

public class CreateExpenseModel
{
    [Required]
    [Display(Name = "Expense Name")]
    public string ExpenseName { get; set; } = default!;

    [Required]
    [Display(Name = "Expense Amount")]
    public decimal ExpenseAmount { get; set; }

    [Required]
    [Display(Name = "Expense Description")]
    public string ExpenseDescription { get; set; } = default!;

    [Required]
    [Display(Name = "Expense Currency")]
    public Currency ExpenseCurrencyUsed { get; set; } = default!;

    [Required]
    [Display(Name = "Notes on Expense")]
    public string ExpenseNotes { get; set; } = default!;

    [Required]
    [Display(Name = "Expense Type")]
    public ExpenseType ExpenseType { get; set; }

    [Required]
    [Display(Name = "Expense Date")]
    public DateOnly ExpenseDate { get; set; }
}

public class CreateExpenseInputModel
{
    [Required]
    [Display(Name = "Expense Name")]
    public string ExpenseName { get; set; } = default!;

    [Required]
    [Display(Name = "Expense Amount")]
    public decimal ExpenseAmount { get; set; }

    [Required]
    [Display(Name = "Expense Description")]
    public string ExpenseDescription { get; set; } = default!;

    [Required]
    [Display(Name = "Expense Type")]
    public ExpenseType ExpenseType { get; set; }

    [Required]
    [Display(Name = "Expense Type Selected")]
    public int ExpenseTypeSelected { get; set; }

    public Currency CurrencyUsed { get; set; }

    [Required]
    [Display(Name = "Expense Date")]
    public DateOnly ExpenseDate { get; set; }

    // [Required]
    // [Display(Name = "Expense Owner")]
    // public string ExpenseOwnerId { get; set; } = default!;
}

public record ExpenseIndexModel(string ExpenseName, string ExpenseDescription, decimal ExpenseAmount,
    DateOnly ExpenseDate, Guid Id, Guid BudgetId, Currency CurrencyUsed, ExpenseType ExpenseType, string ExpenseNotes, 
    string ExpenseOwnerEmail);

public record ExpenseUpdateModel(string ExpenseName, string ExpenseDescription, decimal Amount, DateOnly ExpenseDate,
    Guid ExpenseId, Currency CurrencyUsed, ExpenseType ExpenseType, string Notes, string ExpenseOwnerEmail);

public record ExpenseDashBoardSummary(decimal Amount, Currency CurrencyUsed);

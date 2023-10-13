using Microsoft.AspNetCore.Identity;
using NodaTime;

namespace CashPurse.Server.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Expenses = new List<Expense>();
        Incomes = new List<Income>();
        CreatedAt = DateOnly.FromDateTime(DateTime.Today);
        UpdatedAt = DateOnly.FromDateTime(DateTime.Today);
    }

     public DateOnly CreatedAt { get; set; }

    public DateOnly UpdatedAt { get; set; }

    public Currency PreferredCurrency { get; set; }

    public string FullName { get; set; } = string.Empty;

    public List<Expense> Expenses { get; set; } = null!;

    public List<Income> Incomes { get; set; } = null!;
}

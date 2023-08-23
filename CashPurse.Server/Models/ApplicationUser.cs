using Microsoft.AspNetCore.Identity;

namespace CashPurse.Server.Models;

public class ApplicationUser : IdentityUser
{
    public ApplicationUser()
    {
        Expenses = new List<Expense>();
        Incomes = new List<Income>();
    }

    public Currency PreferredCurrency { get; set; }

    public string FullName { get; set; } = string.Empty;

    public List<Expense> Expenses { get; set; } = null!;

    public List<Income> Incomes { get; set; } = null!;
}

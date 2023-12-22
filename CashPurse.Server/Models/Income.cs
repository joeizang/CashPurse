namespace CashPurse.Server.Models;

public class Income : BaseEntity
{
    public decimal Amount { get; set; }

    public Currency CurrencyUsed { get; set; }

    public string? Notes { get; set; } = string.Empty;

    // public ApplicationUser IncomeOwner { get; set; } = default!;
    //
    // public required string IncomeOwnerId { get; set; } = string.Empty;
}

using CashPurse.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CashPurse.Server.Data;

public class CashPurseDbContext : IdentityDbContext<ApplicationUser>
{
    public CashPurseDbContext(DbContextOptions<CashPurseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Expense> Expenses { get; set; } = null!;

    public DbSet<Income> Incomes { get; set; } = null!;

    public DbSet<BudgetList> BudgetLists { get; set; } = null!;

    public DbSet<BudgetListItem> BudgetListItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Expense>()
            .Property(e => e.Amount)
            .HasPrecision(18, 2);
        builder.Entity<Income>()
            .Property(i => i.Amount)
            .HasPrecision(18, 2);
        builder.Entity<BudgetListItem>()
            .Property(b => b.UnitPrice)
            .HasPrecision(18, 2);
        builder.Entity<BudgetListItem>()
            .Property(b => b.Price)
            .HasPrecision(18, 2);

        builder.Entity<Expense>()
            .HasMany(e => e.ExpenseBudget)
            .WithOne()
            .HasForeignKey(x => x.ExpenseEntityId)
            .IsRequired(false);

        builder.Entity<Expense>()
            .HasIndex(e => e.ExpenseDate);
        builder.Entity<Expense>()
            .HasIndex(e => e.Amount);
        builder.Entity<BudgetList>()
            .HasIndex(b => b.CreatedAt);


        base.OnModelCreating(builder);
    }
}

using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.BusinessLogic.EndpointHandlers;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CashPurseServerTests.DataTests;

public class BudgetListDataServiceTest
{
    [Fact]
    public async void CreateBudgetListWithDataService_CreatesABudgetList()
    {
        var sqlite = new SqliteConnection("Filename=:memory:");
        await sqlite.OpenAsync();
        var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
        var options = optionsBuilder.UseSqlite(sqlite).Options;
        await using var context = new CashPurseDbContext(options);
        var userId = Guid.NewGuid().ToString("D");
        await context.Database.EnsureCreatedAsync();

        var budgetList = new BudgetList
        {
            ListName = "10th Year anniversary party budget",
            Description = "planning for a party for our anniversary",
            // OwnerId = userId,
            BudgetItems = new()
        };

        await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
        
        Assert.NotEmpty(context.BudgetLists.ToList());
        Assert.Equal(1, context.BudgetLists.Count());
    }

    [Fact]
    public async void CreateBudgetListWithBudgetItems()
    {
        var sqlite = new SqliteConnection("Filename=:memory:");
        await sqlite.OpenAsync();
        var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
        var options = optionsBuilder.UseSqlite(sqlite).Options;
        await using var context = new CashPurseDbContext(options);
        var userId = Guid.NewGuid().ToString("D");
        await context.Database.EnsureCreatedAsync();

        var budgetList = new BudgetList
        {
            ListName = "10th Year anniversary party budget",
            Description = "planning for a party for our anniversary",
            // OwnerId = userId,
            BudgetItems = new()
        };

        var budgetItem = new BudgetListItem
        {
            Name = "Cake",
            Description = "A cake for the party",
            UnitPrice = 100m,
            Quantity = 1
        };
        budgetItem.CalculateItemPrice();

        budgetList.BudgetItems.Add(budgetItem);

        await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
        
        Assert.NotEmpty(context.BudgetLists.ToList());
        Assert.Equal(1, context.BudgetLists.Count());
        Assert.Single(context.BudgetLists.AsNoTracking().Include(x => x.BudgetItems).Single().BudgetItems);

        await context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async void UpdateBudgetListWithDataService()
    {
        var sqlite = new SqliteConnection("Filename=:memory:");
        await sqlite.OpenAsync();
        var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
        var options = optionsBuilder.UseSqlite(sqlite).Options;
        await using var context = new CashPurseDbContext(options);
        var userId = Guid.NewGuid().ToString("D");
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var budgetList = new BudgetList
        {
            ListName = "10th Year anniversary party budget",
            Description = "planning for a party for our anniversary",
            // OwnerId = userId,
            BudgetItems = new()
        };

        var budgetItem = new BudgetListItem
        {
            Name = "Cake",
            Description = "A cake for the party",
            UnitPrice = 100m,
            Quantity = 1
        };
        budgetItem.CalculateItemPrice();

        budgetList.BudgetItems.Add(budgetItem);

        await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);
        
        Assert.NotEmpty(context.BudgetLists.ToList());
        Assert.Equal(1, context.BudgetLists.Count());
        Assert.Single(context.BudgetLists.AsNoTracking().Include(x => x.BudgetItems).Single().BudgetItems);
        
        var budgetListToUpdate = context.BudgetLists.Include(x => x.BudgetItems).Single();
        budgetListToUpdate.ListName = "Updated List Name";
        budgetListToUpdate.Description = "Updated Description";
        budgetListToUpdate.BudgetItems.Single().Name = "Updated Item Name";
        budgetListToUpdate.BudgetItems.Single().Description = "Updated Item Description";
        budgetListToUpdate.BudgetItems.Single().UnitPrice = 200m;
        budgetListToUpdate.BudgetItems.Single().Quantity = 2;
        budgetListToUpdate.BudgetItems.Single().CalculateItemPrice();

        await BudgetListDataService.UpdateBudgetList(context, budgetListToUpdate).ConfigureAwait(false);
        
        Assert.NotEmpty(context.BudgetLists.ToList());
        Assert.Equal(1, context.BudgetLists.Count());
        Assert.Single(context.BudgetLists.AsNoTracking().Include(x => x.BudgetItems).Single().BudgetItems);
        Assert.Equal("Updated List Name", context.BudgetLists.AsNoTracking().Include(x => x.BudgetItems).Single().ListName);
        Assert.Equal("Updated Description", context.BudgetLists.AsNoTracking().Include(x => x.BudgetItems).Single().Description);

        await context.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async void GetBudgetListsWithDataService()
    {
        var sqlite = new SqliteConnection("Filename=:memory:");
        await sqlite.OpenAsync();
        var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
        var options = optionsBuilder.UseSqlite(sqlite).Options;
        await using var context = new CashPurseDbContext(options);
        var userId = Guid.NewGuid().ToString("D");
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var budgetList = new BudgetList
        {
            ListName = "10th Year anniversary party budget",
            Description = "planning for a party for our anniversary",
            // OwnerId = userId,
            BudgetItems = new()
        };

        var budgetItem = new BudgetListItem
        {
            Name = "Cake",
            Description = "A cake for the party",
            UnitPrice = 100m,
            Quantity = 1
        };
        // budgetItem.CalculateItemPrice();

        budgetList.BudgetItems.Add(budgetItem);

        await BudgetListDataService.AddNewBudgetList(context, budgetList).ConfigureAwait(false);

        Assert.NotEmpty(context.BudgetLists.ToList());
        Assert.Equal(1, context.BudgetLists.Count());
        Assert.Single(context.BudgetLists.AsNoTracking().Include(x => x.BudgetItems).Single().BudgetItems);

        var budgetListToUpdate = context.BudgetLists.Include(x => x.BudgetItems).Single();
        budgetListToUpdate.ListName = "Updated List Name";
        budgetListToUpdate.Description = "Updated Description";
        budgetListToUpdate.BudgetItems.Single().Name = "Updated Item Name";
        budgetListToUpdate.BudgetItems.Single().Description = "Updated Item Description";
        budgetListToUpdate.BudgetItems.Single().UnitPrice = 200m;
        budgetListToUpdate.BudgetItems.Single().Quantity = 2;

        await BudgetListDataService.UpdateBudgetList(context, budgetListToUpdate).ConfigureAwait(false);
        var ans = await BudgetListDataService.UserBudgetLists(context);
        Assert.NotEmpty(context.BudgetLists.ToList());
        Assert.Equal(1, context.BudgetLists.Count());
        Assert.Collection(ans.Items);
    }
}
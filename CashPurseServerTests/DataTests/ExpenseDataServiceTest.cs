using CashPurse.Server.BusinessLogic.DataServices;
using CashPurse.Server.Data;
using CashPurse.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CashPurse.Server.ApiModels;

namespace CashPurseServerTests.DataTests
{
    public class ExpenseDataServiceTest
    {

        [Fact]
        public async void CreateExpenseWithDataService_CreatesExpenseInDB()
        {
            //Arrange
            // var userId = Guid.NewGuid().ToString("D");
            // var sqlitedb = new SqliteConnection("Filename=:memory:");
            // sqlitedb.Open();
            // var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
            // var options = optionsBuilder.UseSqlite(sqlitedb).Options;
            // await using var db = new CashPurseDbContext(options);
            // if(db.Database.EnsureCreated())
            // {
            //     Console.WriteLine("Database created");
            // }
            // var expense = new Expense
            // {
            //     Name = "Buy a car for Ushim",
            //     Description = "Bought a Toyota Camry for Ushim",
            //     ExpenseDate = DateOnly.FromDateTime(DateTime.Today),
            //     Amount = 30000000,
            //     CurrencyUsed = Currency.NGN,
            //     ExpenseType = ExpenseType.Transportation,
            //     // ExpenseOwnerId = userId,
            //     Id = Guid.NewGuid()
            // };
            // //Act
            // await ExpenseDataService.AddNewExpense(db, expense, TODO).ConfigureAwait(false);
            // //Assert
            // Assert.NotEmpty(db.Expenses.ToList());
        }

        [Fact]
        public async void UpdateExpenseWithDataService_UpdatesExpenseInDB()
        {
            //Arrange
            // var userId = Guid.NewGuid().ToString("D");
            // var sqlitedb = new SqliteConnection("Filename=:memory:");
            // sqlitedb.Open();
            // var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
            // var options = optionsBuilder.UseSqlite(sqlitedb).Options;
            // await using var db = new CashPurseDbContext(options);
            // if (db.Database.EnsureCreated())
            // {
            //     Console.WriteLine("Database created");
            // }
            // var expense = new Expense
            // {
            //     Name = "Buy a car for Ushim",
            //     Description = "Bought a Toyota Camry for Ushim",
            //     ExpenseDate = DateOnly.FromDateTime(DateTime.Today),
            //     Amount = 30000000,
            //     CurrencyUsed = Currency.NGN,
            //     ExpenseType = ExpenseType.Transportation,
            //     // ExpenseOwnerId = userId,
            //     Id = Guid.NewGuid()
            // };
            // //Act
            // await ExpenseDataService.AddNewExpense(db, expense, TODO).ConfigureAwait(false);
            // var expenseupdate = await db.Expenses.Where(x => x.Amount == 30000000)
            //     .SingleAsync()
            //     .ConfigureAwait(false);
            // var updateModel = new UpdateExpenseRequest(expenseupdate.Id, "Buy a car for Ushim My Love",
            //     expenseupdate.Description, expenseupdate.Amount, expenseupdate.ExpenseDate, expenseupdate.ExpenseType,
            //     expenseupdate.CurrencyUsed, /*expenseupdate.ExpenseOwnerId,*/ expenseupdate.Notes);
            // expenseupdate.Name = "Buy a car for Ushim My Love";
            // await ExpenseDataService.UpdateExpense(db, expenseupdate, updateModel).ConfigureAwait(false);
            // //Assert
            // Assert.Equal("Buy a car for Ushim My Love", db.Expenses.Single().Name);
            // Assert.EndsWith("Love", db.Expenses.Single().Name);
        }
    }
}

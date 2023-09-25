﻿using CashPurse.Server.BusinessLogic.DataServices;
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

namespace CashPurseServerTests.DataTests
{
    public class ExpenseDataServiceTest
    {
        // private readonly IServiceScope _scope;
        // public ExpenseDataServiceTest()
        // {
        //     var serviceCollection = new ServiceCollection();
        //     serviceCollection.AddScoped<UserManager<ApplicationUser>>();

        //     var provider = serviceCollection.BuildServiceProvider();
        //     _scope = provider.CreateScope();
            
        // }


        [Fact]
        public async void CreateExpenseWithDataService_CreatesExpenseInDB()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString("D");
            var sqlitedb = new SqliteConnection("Filename=:memory:");
            sqlitedb.Open();
            var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
            var options = optionsBuilder.UseSqlite(sqlitedb).Options;
            await using var db = new CashPurseDbContext(options);
            if(db.Database.EnsureCreated())
            {
                Console.WriteLine("Database created");
            }
            var expense = new Expense
            {
                Name = "Buy a car for Ushim",
                Description = "Bought a Toyota Camry for Ushim",
                ExpenseDate = DateTime.Now,
                Amount = 30000000,
                CurrencyUsed = Currency.NGN,
                ExpenseType = ExpenseType.Transportation,
                ExpenseOwnerId = userId,
                Id = Guid.NewGuid()
            };
            //Act
            await ExpenseDataService.AddNewExpense(db, expense).ConfigureAwait(false);
            //Assert
            Assert.NotEmpty(db.Expenses.ToList());
        }

        [Fact]
        public async void UpdateExpenseWithDataService_UpdatesExpenseInDB()
        {
            //Arrange
            var userId = Guid.NewGuid().ToString("D");
            var sqlitedb = new SqliteConnection("Filename=:memory:");
            sqlitedb.Open();
            var optionsBuilder = new DbContextOptionsBuilder<CashPurseDbContext>();
            var options = optionsBuilder.UseSqlite(sqlitedb).Options;
            await using var db = new CashPurseDbContext(options);
            if (db.Database.EnsureCreated())
            {
                Console.WriteLine("Database created");
            }
            var expense = new Expense
            {
                Name = "Buy a car for Ushim",
                Description = "Bought a Toyota Camry for Ushim",
                ExpenseDate = DateTime.Now,
                Amount = 30000000,
                CurrencyUsed = Currency.NGN,
                ExpenseType = ExpenseType.Transportation,
                ExpenseOwnerId = userId,
                Id = Guid.NewGuid()
            };
            //Act
            await ExpenseDataService.AddNewExpense(db, expense).ConfigureAwait(false);
            var expenseupdate = await db.Expenses.Where(x => x.Amount == 30000000).SingleAsync().ConfigureAwait(false);
            expenseupdate.Name = "Buy a car for Ushim My Love";
            await ExpenseDataService.UpdateExpense(db, expense).ConfigureAwait(false);
            //Assert
            Assert.Equal("Buy a car for Ushim My Love", db.Expenses.Single().Name);
            Assert.EndsWith("Love", db.Expenses.Single().Name);
        }
    }
}

using CashPurse.Server.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashPurseServerTests.DataTests
{
    public class ExpenseDataServiceTest
    {
        [Fact]
        public async void CreateExpenseWithDataService_CreatesExpenseInDB()
        {
            //Arrange
            var options = new DbContextOptions<CashPurseDbContext>();
            await using var db = new CashPurseDbContext(options);

            //Act


            //Assert
        }
    }
}

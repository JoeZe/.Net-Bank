using Microsoft.VisualStudio.TestTools.UnitTesting;
using NetBankAppV1;
using NetBankAppV1.Controllers;
using NetBankAppV1.Data;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetBankAppV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UnitTesting
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DbContextOptions<BankDbContext> options = new DbContextOptionsBuilder<BankDbContext>()
                .UseInMemoryDatabase(databaseName: "TestMethod1")
                .Options;
            var context = new BankDbContext(options);

            Customer customers = new Customer
            {
                FirstName = "Sam",
                LastName = "lee",
                Address = "21811"
            };

            context.customers.Add(customers);

            context.SaveChanges();

            context.Database.EnsureDeleted();
            context.Dispose();
            //    CheckingAccount ck = new CheckingAccount
            //    {
            //        AccountNumber = 100,
            //        Balance = 1000

            //    };

            //    Assert.AreEqual(1000, ck.Balance);

            //}


            // Act



            // Assert                
            //Assert.Equals("TestCategory", context.ResourceCategories.First().Name);
            //AccountsController accountController = new AccountsController(new NetBankAppV1.Data.BankDbContext());
        }

    
}

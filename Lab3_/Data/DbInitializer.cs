using System;
using System.Linq;
using Customs.DAL;
using Customs.DAL.Models;

namespace Lab3_.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CustomsContext db)
        {
            db.Database.EnsureCreated();

            if (db.Employees.Any()) return;

            const int employeesNumber = 35;
            const int storageNumber = 35;
            const int productNumber = 300;

            var employees = Enumerable.Range(1, employeesNumber)
                .Select(employeeId => new Employee
                {
                    FirstName = "TestFirstName" + employeeId,
                    LastName = "TestLastName" + employeeId,
                    MiddleName = "TestMiddleName" + employeeId,
                    IdNumber = employeeId.ToString(),
                    Role = "TestRole" + employeeId,
                })
                .ToList();
            db.Employees.AddRange(employees);
            db.SaveChanges();

            var storages = Enumerable.Range(1, storageNumber)
                .Select(storageId => new Storage
                {
                    Name = "TestStorage" + storageId,
                })
                .ToList();
            db.Storages.AddRange(storages);
            db.SaveChanges();

            var products = Enumerable.Range(1, productNumber)
                .Select(productId => new Product
                {
                    Name = "TestProduct" + productId,
                    UnitMeasurement = new Random().Next(1, 100),
                    StorageId = new Random().Next(1, storageNumber)
                })
                .ToList();

            db.Products.AddRange(products);
            db.SaveChanges();
        }
    }
}
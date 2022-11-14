using System;
using System.Collections.Generic;
using System.Linq;
using Customs.DAL;
using Lab3_.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Lab3_.Services
{
    // Класс выборки 10 записей из таблиц 
    public class ProductsService : IProductsService
    {
        private readonly CustomsContext _db;
        private readonly IMemoryCache _cache;

        public readonly int NumberRows = 20;

        public ProductsService(CustomsContext db, IMemoryCache cache)
        {
            _db = db;
            _cache = cache;
        }

        public HomeViewModel GetHomeViewModel(string cacheKey)
        {
            if (_cache.TryGetValue(cacheKey, out HomeViewModel result))
            {
                return result;
            }
            var storages = _db.Storages.AsNoTracking().Take(NumberRows).ToList();
            var employees = _db.Employees.AsNoTracking().Take(NumberRows).ToList();
            var products = _db.Products
                .Include(t => t.Storage)
                .Select(t => new ProductViewModel
                {
                    Id = t.Id,
                    UnitMeasurement = t.UnitMeasurement,
                    Name = t.Name,
                    Storage = t.Storage.Name
                })
                .Take(NumberRows)
                .ToList();

            var homeViewModel = new HomeViewModel
            {
                Storages = storages,
                Employees = employees,
                Products = products
            };
            _cache.Set(cacheKey, result,
                new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(2 * 26 + 240)));
            return homeViewModel;
        }

        public List<string> GetStorages()
        {
            var storages = _db.Storages
                .Select(x => x.Name)
                .Distinct()
                .ToList();

            return storages;
        }

        public List<ProductViewModel> SearchProducts(string storage, string name)
        {
            storage = storage.ToLower();
            name = name.ToLower();

            var products = _db.Products
                .Include(t => t.Storage)
                .Where(x => x.Storage.Name.ToLower().StartsWith(storage) && x.Name.ToLower().StartsWith(name))
                .Select(t => new ProductViewModel
                {
                    Id = t.Id,
                    UnitMeasurement = t.UnitMeasurement,
                    Name = t.Name,
                    Storage = t.Storage.Name
                })
                .ToList();

            return products;
        }
    }
}
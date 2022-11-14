using System.Collections.Generic;
using Lab3_.ViewModels;

namespace Lab3_.Services
{
    public interface IProductsService
    {
        HomeViewModel GetHomeViewModel(string cacheKey);
        List<string> GetStorages();

        List<ProductViewModel> SearchProducts(string storage, string name);
    }
}
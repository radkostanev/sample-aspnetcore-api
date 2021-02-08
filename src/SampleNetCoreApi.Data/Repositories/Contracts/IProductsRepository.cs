using SampleNetCoreApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleNetCoreApi.Data.Repositories.Contracts
{
    public interface IProductsRepository
    {
        int Count();
        Product GetSingle(int id);
        IEnumerable<Product> GetAll();
        void Add(Product product);
        void Delete(int id);
        Product Update(Product product);
        bool Save();
    }
}

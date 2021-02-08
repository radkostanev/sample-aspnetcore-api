using SampleNetCoreApi.Data.Context;
using SampleNetCoreApi.Data.Entities;
using SampleNetCoreApi.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleNetCoreApi.Data.Repositories
{
    public class ProductsRepository : IProductsRepository
    {
        private readonly ProductsContext context;

        public ProductsRepository(ProductsContext context)
        {
            this.context = context;
        }

        public int Count() => this.context.Products.Count();

        public Product GetSingle(int id) => this.context.Products.FirstOrDefault(product => product.Id == id);

        public IEnumerable<Product> GetAll() => this.context.Products.AsEnumerable();

        public void Add(Product product) => this.context.Products.Add(product);

        public void Delete(int id)
        {
            var product = GetSingle(id);
            this.context.Products.Remove(product);
        }

        public Product Update(Product product)
        {
            this.context.Products.Update(product);
            return product;
        }

        public bool Save() => this.context.SaveChanges() >= 0;
    }
}

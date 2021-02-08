using BCrypt.Net;
using SampleNetCoreApi.Data.Context;
using SampleNetCoreApi.Data.Entities;
using SampleNetCoreApi.Services.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SampleNetCoreApi.Services.Services
{
    public class SeedService : ISeedService
    {
        public async Task Seed(ProductsContext context)
        {
            var products = new List<Product> 
            {
                new Product { Id = 1, Name = "GPU", Price = 500m, Category = "Electronics", CreatedOn = DateTime.UtcNow },
                new Product { Id = 2, Name = "CPU", Price = 400.99m, Category = "Electronics", CreatedOn = DateTime.UtcNow },
                new Product { Id = 3, Name = "Motherboard", Price = 200m, Category = "Electronics", CreatedOn = DateTime.UtcNow },
                new Product { Id = 4, Name = "RAM", Price = 99.99m, Category = "Electronics", CreatedOn = DateTime.UtcNow },
                new Product { Id = 5, Name = "SSD", Price = 250.49m, Category = "Electronics", CreatedOn = DateTime.UtcNow },
            };
            context.Products.AddRange(products);

            var user = new User { Id = 1, FirstName = "Bruce", LastName = "Banner", Username = "bruce", PasswordHash = BCrypt.Net.BCrypt.HashPassword("banner") };
            context.Users.Add(user);

            await context.SaveChangesAsync();
        }
    }
}

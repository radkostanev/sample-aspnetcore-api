using Microsoft.EntityFrameworkCore;
using SampleNetCoreApi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleNetCoreApi.Data.Context
{
    public class ProductsContext : DbContext
    {
        public ProductsContext(DbContextOptions<ProductsContext> options)
           : base(options) 
        {}

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

    }
}

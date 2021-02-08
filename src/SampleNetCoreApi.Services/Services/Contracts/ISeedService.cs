using SampleNetCoreApi.Data.Context;
using System;
using System.Threading.Tasks;

namespace SampleNetCoreApi.Services.Services.Contracts
{
    public interface ISeedService
    {
        Task Seed(ProductsContext context);
    }
}

using SampleNetCoreApi.Data.Entities;
using SampleNetCoreApi.Services.DTOs;
using SampleNetCoreApi.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleNetCoreApi.Services.Services.Contracts
{
    public interface IProductsService
    {
        int Count();
        ProductDTO GetSingle(int id);
        IEnumerable<ProductDTO> GetAll(QueryParams queryParams);
        ProductDTO Add(ProductCreateDTO productCreateDTO);
        void Delete(int id);
        ProductDTO Update(int id, ProductUpdateDTO productUpdateDTO);
        bool Save();
    }
}

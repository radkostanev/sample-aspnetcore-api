using AutoMapper;
using SampleNetCoreApi.Data.Entities;
using SampleNetCoreApi.Data.Repositories.Contracts;
using SampleNetCoreApi.Services.DTOs;
using SampleNetCoreApi.Services.Helpers;
using SampleNetCoreApi.Services.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SampleNetCoreApi.Services.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsRepository productsRepository;
        private readonly IMapper mapper;

        public ProductsService(IProductsRepository productsRepository, IMapper mapper)
        {
            this.productsRepository = productsRepository;
            this.mapper = mapper;
        }

        public int Count() => this.productsRepository.Count();

        public ProductDTO GetSingle(int id)
        {
            var product = this.productsRepository.GetSingle(id);

            return this.mapper.Map<ProductDTO>(product);
        }

        public IEnumerable<ProductDTO> GetAll(QueryParams queryParams)
        {
            var result = this.productsRepository.GetAll()
                .OrderBy(GetOrder(queryParams.OrderBy))
                .Select(product => this.mapper.Map<ProductDTO>(product));

            if (!string.IsNullOrEmpty(queryParams.Query))
            {
                result = (IOrderedEnumerable<ProductDTO>)result
                    .Where(productDTO => productDTO.Name.Contains(queryParams.Query.ToLowerInvariant())
                    || productDTO.Category.Contains(queryParams.Query.ToLowerInvariant())
                    || productDTO.Price.ToString().Contains(queryParams.Query));
            }

            return result.Skip(queryParams.PageCount * (queryParams.Page - 1))
                .Take(queryParams.PageCount);
        }


        public ProductDTO Add(ProductCreateDTO productCreateDTO)
        {
            var newProduct = this.mapper.Map<Product>(productCreateDTO);
            this.productsRepository.Add(newProduct);
            return this.mapper.Map<ProductDTO>(newProduct);
        }

        public void Delete(int id) => this.productsRepository.Delete(id);

        public ProductDTO Update(int id, ProductUpdateDTO productUpdateDTO)
        {
            var productToUpdate = this.productsRepository.GetSingle(id);

            if (productToUpdate == null)
            {
                return null;
            }

            this.mapper.Map(productUpdateDTO, productToUpdate);
            this.productsRepository.Update(productToUpdate);

            var productDTO = this.mapper.Map<ProductDTO>(productToUpdate);
            return productDTO;
        }

        public bool Save() => this.productsRepository.Save();

        private static Func<Product, IComparable> GetOrder(string order) => order switch
        {
            "id" => product => product.Id,
            "name" => product => product.Name,
            "price" => product => product.Price,
            "category" => product => product.Category,
            _ => product => product.Name,
        };
    }
}

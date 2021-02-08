using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SampleNetCoreApi.Api.Helpers;
using SampleNetCoreApi.Api.Models;
using SampleNetCoreApi.Services.DTOs;
using SampleNetCoreApi.Services.Helpers;
using SampleNetCoreApi.Services.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace SampleNetCoreApi.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IProductsService productsService;
        private readonly IUrlHelper urlHelper;

        public ProductsController(IMapper mapper, IProductsService productsService, IUrlHelper urlHelper)
        {
            this.mapper = mapper;
            this.productsService = productsService;
            this.urlHelper = urlHelper;
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleProduct))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetSingleProduct(int id)
        {
            var productDTO = this.productsService.GetSingle(id);

            if (productDTO == null)
            {
                return NotFound();
            }

            return Ok(ExpandSingleProduct(productDTO));
        }

        [HttpGet(Name = nameof(GetProducts))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult GetProducts([FromQuery] QueryParams queryParams)
        {
            var itemsCount = this.productsService.Count();

            var paginationMetadata = new
            {
                totalCount = itemsCount,
                pageSize = queryParams.PageCount,
                currentPage = queryParams.Page,
                totalPages = queryParams.GetTotalPages(itemsCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var data = this.productsService.GetAll(queryParams)
                .Select(x => ExpandSingleProduct(x));

            return Ok(new
            {
                value = data,
                links = CreateLinksForCollection(queryParams, itemsCount)
            });
        }

        [HttpPost(Name = nameof(CreateProduct))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateProduct([FromBody] ProductCreateDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var productDTO = this.productsService.Add(model);

            if (!this.productsService.Save())
            {
                return BadRequest("Saving to database failed.");
            }

            return CreatedAtRoute(nameof(GetSingleProduct), new { id = productDTO.Id }, productDTO);
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(DeleteProduct))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult DeleteProduct(int id)
        {
            this.productsService.Delete(id);

            if (!this.productsService.Save())
            {
                return BadRequest("Saving to database failed.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateProduct))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateProduct(int id, [FromBody] ProductUpdateDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }

            var updatedProductDTO = this.productsService.Update(id, model);

            if (updatedProductDTO == null || !this.productsService.Save())
            {
                return BadRequest();
            }

            return Ok(updatedProductDTO);
        }

        [NonAction]
        private dynamic ExpandSingleProduct(ProductDTO model)
        {
            var links = GetLinks(model.Id);

            var resourceToReturn = model.ToDynamic() as IDictionary<string, object>;
            resourceToReturn.Add("links", links);

            return resourceToReturn;
        }

        [NonAction]
        private IEnumerable<Link> GetLinks(int id)
        {
            var links = new List<Link>();

            var getLink = this.urlHelper.Link(nameof(GetSingleProduct), new { id = id });

            links.Add(
              new Link(getLink, "self", "GET"));

            var deleteLink = this.urlHelper.Link(nameof(DeleteProduct), new { id = id });

            links.Add(
              new Link(deleteLink,
              "delete_product",
              "DELETE"));

            var createLink = this.urlHelper.Link(nameof(CreateProduct), new { });

            links.Add(
              new Link(createLink,
              "create_product",
              "POST"));

            var updateLink = this.urlHelper.Link(nameof(UpdateProduct), new { id = id });

            links.Add(
               new Link(updateLink,
               "update_product",
               "PUT"));

            return links;
        }

        [NonAction]
        private List<Link> CreateLinksForCollection(QueryParams queryParams, int totalCount)
        {
            var links = new List<Link>();

            links.Add(new Link(this.urlHelper.Link(nameof(GetProducts), new
            {
                pagecount = queryParams.PageCount,
                page = queryParams.Page,
                orderby = queryParams.OrderBy
            }), "self", "GET"));

            links.Add(new Link(this.urlHelper.Link(nameof(GetProducts), new
            {
                pagecount = queryParams.PageCount,
                page = 1,
                orderby = queryParams.OrderBy
            }), "first", "GET"));

            links.Add(new Link(this.urlHelper.Link(nameof(GetProducts), new
            {
                pagecount = queryParams.PageCount,
                page = queryParams.GetTotalPages(totalCount),
                orderby = queryParams.OrderBy
            }), "last", "GET"));

            if (queryParams.HasNext(totalCount))
            {
                links.Add(new Link(this.urlHelper.Link(nameof(GetProducts), new
                {
                    pagecount = queryParams.PageCount,
                    page = queryParams.Page + 1,
                    orderby = queryParams.OrderBy
                }), "next", "GET"));
            }

            if (queryParams.HasPrevious())
            {
                links.Add(new Link(this.urlHelper.Link(nameof(GetProducts), new
                {
                    pagecount = queryParams.PageCount,
                    page = queryParams.Page - 1,
                    orderby = queryParams.OrderBy
                }), "previous", "GET"));
            }

            return links;
        }
    }
}

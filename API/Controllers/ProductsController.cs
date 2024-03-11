using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Core.Secifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> productsRepository;
        private readonly IGenericRepository<ProductBrand> productBrandRepoitory;
        private readonly IGenericRepository<ProductType> productTypeRepository;
        private readonly IMapper mapper;

        public ProductsController(
            IGenericRepository<Product> productsRepository,
            IGenericRepository<ProductBrand> productBrandRepoitory,
            IGenericRepository<ProductType> productTypeRepository,
            IMapper mapper)
        {
            this.productsRepository = productsRepository;
            this.productBrandRepoitory = productBrandRepoitory;
            this.productTypeRepository = productTypeRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts()
        {
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await productsRepository.ListAsync(spec);

            return Ok(mapper.Map<IReadOnlyList<Product>, 
                IReadOnlyList<ProductToReturnDto>>(products));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(
            int id)
        {
            var spec = new ProductsWithTypesAndBrandsSpecification(id);
            var product =  await productsRepository
                .GetEntityWithSpec(spec);

            if (product == null) 
                return NotFound(new ApiResponse(404));

            return mapper.Map<Product, ProductToReturnDto>(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await productBrandRepoitory.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            return Ok(await productTypeRepository.ListAllAsync());
        }
    }
}
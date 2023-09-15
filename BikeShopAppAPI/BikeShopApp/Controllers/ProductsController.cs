using AutoMapper;
using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using BikeShopApp.Core.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace BikeShopApp.WebAPI.Controllers
{
    public class ProductsController : CustomControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Products Controller Constructor.
        /// </summary>
        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the product with the passed Id.
        /// </summary>
        /// <param name="productId"></param>
        [HttpGet("{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProduct(int productId) 
        {
            if (!await _productRepository.ProductExistsAsync(productId)) 
            {
                return Problem(detail: $"No product with the Id of {productId} was found.", statusCode: 404, title: "Not Found");
            }

            ProductDto productDto = _mapper.Map<ProductDto>(await _productRepository.GetProductAsync(productId));

            if (productDto == null) 
            {
                return Problem(detail: "Something went wrong while getting the product.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDto);
        }

        /// <summary>
        /// Get all products with the passed category.
        /// </summary>
        /// <param name="category"></param>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromQuery]string? category)
        {
            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(await _productRepository.GetProductsAsync(category));

            if (productDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the products.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDtos);
        }

        /// <summary>
        /// Get all products with the passed category for the current page.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageResults"></param>
        [HttpGet("pages")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsByPage([FromQuery] string? categoryId, string currentPage, string pageResults)
        {
            var productsPageResponse = await _productRepository.GetProductsByPageAsync(categoryId, currentPage, pageResults);

            if (productsPageResponse == null)
            {
                return Problem(detail: "Something went wrong while getting the products by page.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productsPageResponse);
        }

        /// <summary>
        /// Get all products with the passed category that match the price and rating filter.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageResults"></param>
        /// <param name="price"></param>
        /// <param name="rating"></param>
        [HttpGet("pages/filter")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFilteredProductsByPage([FromQuery] string? categoryId, string currentPage, string pageResults, string price, string rating)
        {
            var productsPageResponse = await _productRepository.GetFilteredProductsByPageAsync(categoryId, currentPage, pageResults, price, rating);

            if (productsPageResponse == null)
            {
                return Problem(detail: "Something went wrong while getting the filtered products by page.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productsPageResponse);
        }

        /// <summary>
        /// Create a new product based on the passed productDto.
        /// </summary>
        /// <param name="productDto"></param>
        [HttpPost]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            if (productDto == null) 
            {
                return Problem(detail: "No product was passed.", statusCode: 400, title: "Bad Request");
            }

            Product product = _mapper.Map<Product>(productDto);

            if (!await _productRepository.CreateProductAsync(product))
            {
                return Problem(detail: "Something went wrong while creating the product.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Update an existing product based on the passed productDto.
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="productDto"></param>
        [HttpPut("{productId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> UpdateProduct(int productId, ProductDto productDto)
        {
            if (productDto == null)
            {
                return Problem(detail: "No product was passed.", statusCode: 400, title: "Bad Request");
            }

            if (productDto.ProductId != null)
            {
                if (!await _productRepository.ProductExistsAsync(productDto.ProductId.Value)) 
                {
                    return Problem(detail: $"No product with the Id of {productDto.ProductId.Value} was found.", statusCode: 404, title: "Not Found");
                }
            }
            else 
            {
                return Problem(detail: "No Product Id was passed.", statusCode: 400, title: "Bad Request");
            }

            if (productId != productDto.ProductId)
            {
                return Problem(detail: "Route productId doesn't match body productId.", statusCode: 400, title: "Bad Request");
            }

            Product product = _mapper.Map<Product>(productDto);

            if (!await _productRepository.UpdateProductAsync(product))
            {
                return Problem(detail: "Something went wrong while updating the product.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Delete the product with the passed Id.
        /// </summary>
        /// <param name="productId"></param>
        [HttpDelete("{productId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return Problem(detail: $"No product with the Id of {productId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.DeleteProductAsync(productId))
            {
                return Problem(detail: "Something went wrong while deleting the product.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Get all products that have the passed quantity or lower.
        /// </summary>
        /// <param name="quantity"></param>
        [HttpGet("quantity")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsWithQuantityOrLower([FromQuery] int quantity)
        {
            if (quantity < 0)
            {
                return Problem(detail: "Quantity must be greater than or equal to 0.", statusCode: 400, title: "Bad Request");
            }

            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(await _productRepository.GetAllProductsWithQuantityOrLowerAsync(quantity));

            if (productDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the products.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDtos);
        }

        /// <summary>
        /// Get all products whose name starts with the passed letter.
        /// </summary>
        /// <param name="letter"></param>
        [HttpGet("name")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> GetAllProductsThatStartWithLetter([FromQuery] string letter)
        {
            if (letter == null || letter == "")
            {
                return Problem(detail: "A letter was not passed.", statusCode: 400, title: "Bad Request");
            }

            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(await _productRepository.GetAllProductsThatStartWithLetterAsync(letter));

            if (productDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the products.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDtos);
        }

        /// <summary>
        /// Decrease the quantity by 1 of the product with the passed Id.
        /// </summary>
        /// <param name="productId"></param>
        [HttpGet("{productId}/sold")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return Problem(detail: $"No product with the Id of {productId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.DecreaseQuantityAsync(productId))
            {
                return Problem(detail: "Something went wrong while decreasing the quantity.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Set the average rating of the product with the passed Id.
        /// </summary>
        /// <param name="productId"></param>
        [HttpGet("{productId}/rating")]
        [AllowAnonymous]
        public async Task<IActionResult> SetAvgRatingOfProduct(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return Problem(detail: $"No product with the Id of {productId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.SetAvgRatingOfProductAsync(productId)) 
            {
                return Problem(detail: "Something went wrong while setting the average rating of the product.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }
    }
}

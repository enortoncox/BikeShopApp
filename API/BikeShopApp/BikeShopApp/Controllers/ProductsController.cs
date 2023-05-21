using AutoMapper;
using BikeShopApp.Dto;
using BikeShopApp.Interfaces;
using BikeShopApp.Models;
using BikeShopApp.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct(int productId) 
        {
            if (!await _productRepository.ProductExistsAsync(productId)) 
            {
                return NotFound($"No product with the Id of {productId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedProduct = _mapper.Map<ProductDto>(await _productRepository.GetProductAsync(productId));

            if (mappedProduct == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the product.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProduct);
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts([FromQuery]string? category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProducts = _mapper.Map<List<ProductDto>>(await _productRepository.GetProductsAsync(category));

            if (mappedProducts == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the products.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProducts);
        }

        [HttpGet("pages")]
        public async Task<IActionResult> GetProductsByPage([FromQuery] string? categoryId, string currentPage, string pageResults)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productsResponse = await _productRepository.GetProductsByPageAsync(categoryId, currentPage, pageResults);

            if (productsResponse == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the products by page.");
                return StatusCode(500, ModelState);
            }

            return Ok(productsResponse);
        }

        [HttpGet("pages/filter")]
        public async Task<IActionResult> GetFilteredProductsByPage([FromQuery] string? categoryId, string currentPage, string pageResults, string price, string rating)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productsResponse = await _productRepository.GetFilteredProductsByPageAsync(categoryId, currentPage, pageResults, price, rating);

            if (productsResponse == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the filtered products by page.");
                return StatusCode(500, ModelState);
            }

            return Ok(productsResponse);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductDto createdProduct)
        {
            if(createdProduct == null) 
            {
                return BadRequest("No product was passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProduct = _mapper.Map<Product>(createdProduct);


            if (!await _productRepository.CreateProductAsync(mappedProduct))
            {
                ModelState.AddModelError("", "Something went wrong creating the product.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody]ProductDto updatedProduct)
        {
            if (updatedProduct == null)
            {
                return BadRequest("No product was passed.");
            }

            if (updatedProduct.ProductId != null)
            {
                if (!await _productRepository.ProductExistsAsync(updatedProduct.ProductId.Value)) 
                {
                    return NotFound($"No product with the Id of {updatedProduct.ProductId.Value} was found.");
                }
            }
            else 
            {
                return BadRequest(ModelState);
            }

            if (productId != updatedProduct.ProductId)
            {
                return BadRequest("Route productId doesn't match body productId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProduct = _mapper.Map<Product>(updatedProduct);


            if (!await _productRepository.UpdateProductAsync(mappedProduct))
            {
                ModelState.AddModelError("", "Something went wrong updating the product.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return NotFound($"No product with the Id of {productId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _productRepository.DeleteProductAsync(productId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the product.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        [HttpGet("quantity")]
        public async Task<IActionResult> GetAllProductsWithQuantityOrLower([FromQuery] int quantity)
        {
            if (quantity < 0)
            {
                return BadRequest("Quantity must be greater than or equal to 0");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProducts = _mapper.Map<List<ProductDto>>(await _productRepository.GetAllProductsWithQuantityOrLowerAsync(quantity));

            if (mappedProducts == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the products.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProducts);
        }

        [HttpGet("name")]
        public async Task<IActionResult> GetAllProductsThatStartWithLetter([FromQuery] string letter)
        {
            if(letter == null || letter == "")
            {
                return BadRequest("A letter was not passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProducts = _mapper.Map<List<ProductDto>>(await _productRepository.GetAllProductsThatStartWithLetterAsync(letter));

            if (mappedProducts == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the products.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProducts);
        }

        [HttpGet("{productId}/sold")]
        public async Task<IActionResult> DecreaseQuantity(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId)) 
            {
                return NotFound($"A product with the id of {productId} was not found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _productRepository.DecreaseQuantityAsync(productId))
            {
                ModelState.AddModelError("", "Something went wrong decreasing the quantity.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("{productId}/rating")]
        public async Task<IActionResult> SetAvgRatingOfProduct(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return NotFound($"No product with the Id of {productId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            await _productRepository.SetAvgRatingOfProductAsync(productId);

            return Ok();
        }
    }
}

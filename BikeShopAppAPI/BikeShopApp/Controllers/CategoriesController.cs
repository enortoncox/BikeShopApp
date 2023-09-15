using AutoMapper;
using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BikeShopApp.Core.Attributes;

namespace BikeShopApp.WebAPI.Controllers
{
    public class CategoriesController : CustomControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Category Controller Constructor.
        /// </summary>
        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the category of the passed Id.
        /// </summary>
        /// <param name="categoryId"></param>
        [HttpGet("{categoryId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategory(int categoryId) 
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId)) 
            {
                return Problem(detail: $"No category with the Id of {categoryId} was found.", statusCode: 404, title: "Not Found");
            }

            CategoryDto categoryDto = _mapper.Map<CategoryDto>(await _categoryRepository.GetCategoryAsync(categoryId));

            if (categoryDto == null) 
            {
                return Problem(detail: "Something went wrong while getting the category.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(categoryDto);
        }

        /// <summary>
        /// Get all categories in the database.
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategories()
        {
            List<CategoryDto> categoryDtos = _mapper.Map<List<CategoryDto>>(await _categoryRepository.GetCategoriesAsync());

            if (categoryDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the categories.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(categoryDtos);
        }

        /// <summary>
        /// Create a new category based on the passed categoryDto.
        /// </summary>
        /// <param name="categoryDto"></param>
        [HttpPost]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> CreateCategory(CategoryDto categoryDto)
        {
            if (categoryDto == null) 
            {
                return Problem(detail: "No category has been passed.", statusCode: 400, title: "Bad Request");
            }
       
            Category category = _mapper.Map<Category>(categoryDto);

            if (!await _categoryRepository.CreateCategoryAsync(category))
            {
                return Problem(detail: "Something went wrong while creating the category.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Update an existing category based on the passed categoryDto.
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="categoryDto"></param>
        [HttpPut("{categoryId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> UpdateCategory(int categoryId, CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return Problem(detail: "No category has been passed.", statusCode: 400, title: "Bad Request");
            }

            if (categoryDto.CategoryId != null)
            {
                if (!await _categoryRepository.CategoryExistsAsync(categoryDto.CategoryId.Value))
                {
                    return Problem(detail: $"No category with the Id of {categoryDto.CategoryId.Value} was found.", statusCode: 404, title: "Not Found");
                }
            }
            else 
            {
                return Problem(detail: "No category Id has been passed.", statusCode: 400, title: "Bad Request");
            }

            if (categoryId != categoryDto.CategoryId)
            {
                return Problem(detail: "Route categoryId does not match body categoryId.", statusCode: 400, title: "Bad Request");
            }

            Category category = _mapper.Map<Category>(categoryDto);

            if (!await _categoryRepository.UpdateCategoryAsync(category))
            {
                return Problem(detail: "Something went wrong while updating the category.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Deletes the category with the passed Id.
        /// </summary>
        /// <param name="categoryId"></param>
        [HttpDelete("{categoryId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
            {
                return Problem(detail: $"No category with the Id of {categoryId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _categoryRepository.DeleteCategoryAsync(categoryId))
            {
                return Problem(detail: "Something went wrong while deleting the category.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Gets all products that have the passed category Id.
        /// </summary>
        /// <param name="categoryId"></param>
        [HttpGet("{categoryId}/products")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductsByCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId)) 
            {
                return Problem(detail: $"No category with the Id of {categoryId} was found.", statusCode: 404, title: "Not Found");
            }

            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(await _categoryRepository.GetAllProductsByCategoryAsync(categoryId));

            if (productDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the products.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDtos);
        }
    }
}

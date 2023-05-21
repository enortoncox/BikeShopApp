using AutoMapper;
using BikeShopApp.Dto;
using BikeShopApp.Interfaces;
using BikeShopApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BikeShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategory(int categoryId) 
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId)) 
            {
                return NotFound($"No category with the Id of {categoryId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedCategory = _mapper.Map<CategoryDto>(await _categoryRepository.GetCategoryAsync(categoryId));

            if (mappedCategory == null) 
            {
                ModelState.AddModelError("", "Something went wrong when getting the category");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedCategory);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedCategories = _mapper.Map<List<CategoryDto>>(await _categoryRepository.GetCategoriesAsync());

            if (mappedCategories == null)
            {
                ModelState.AddModelError("", "Something went wrong when getting the categories");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedCategories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryDto createdCategory)
        {
            if (createdCategory == null) 
            {
                return BadRequest("No category has been passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
         
            var mappedCategory = _mapper.Map<Category>(createdCategory);

            if (!await _categoryRepository.CreateCategoryAsync(mappedCategory))
            {
                ModelState.AddModelError("", "Something went wrong creating the category");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody]CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
            {
                return BadRequest("No category has been passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updatedCategory.CategoryId != null)
            {
                if (!await _categoryRepository.CategoryExistsAsync(updatedCategory.CategoryId.Value))
                {
                    return NotFound($"No category with the Id of {updatedCategory.CategoryId.Value} was found.");
                }
            }
            else 
            {
                return BadRequest(ModelState);
            }

            if (categoryId != updatedCategory.CategoryId)
            {
                return BadRequest("Route categoryId does not match body categoryId");
            }

            var mappedCategory = _mapper.Map<Category>(updatedCategory);

            if (!await _categoryRepository.UpdateCategoryAsync(mappedCategory))
            {
                ModelState.AddModelError("", "Something went wrong updating the category");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId))
            {
                return NotFound($"No category with the Id of {categoryId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _categoryRepository.DeleteCategoryAsync(categoryId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the category");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        [HttpGet("{categoryId}/products")]
        public async Task<IActionResult> GetAllProductsByCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExistsAsync(categoryId)) 
            {
                return NotFound($"No category with the Id of {categoryId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProducts = _mapper.Map<List<ProductDto>>(await _categoryRepository.GetAllProductsByCategoryAsync(categoryId));

            if (mappedProducts == null)
            {
                ModelState.AddModelError("", "Something went wrong when getting the products");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProducts);
        }
    }
}

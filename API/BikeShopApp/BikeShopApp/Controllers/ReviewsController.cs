using AutoMapper;
using BikeShopApp.Dto;
using BikeShopApp.Interfaces;
using BikeShopApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BikeShopApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ReviewsController(IReviewRepository reviewRepository, IUserRepository userRepository, IProductRepository productRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("{reviewId}")]
        public async Task<IActionResult> GetReview(int reviewId) 
        {
            if (!await _reviewRepository.ReviewExistsAsync(reviewId)) 
            {
                return NotFound($"No review with the Id of {reviewId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedReview = _mapper.Map<ReviewDto>(await _reviewRepository.GetReviewAsync(reviewId));

            if (mappedReview == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the reivew");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedReview);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto createdReview)
        {
            if (createdReview == null)
            {
                return BadRequest("A valid review was not passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _userRepository.UserExistsAsync(createdReview.UserId)) 
            {
                ModelState.AddModelError("", "User was not found!");
                return StatusCode(404, ModelState);
            }

            if (!await _productRepository.ProductExistsAsync(createdReview.ProductId))
            {
                return NotFound($"No product with the id of {createdReview.ProductId} was found.");
            }

            var mappedReview = _mapper.Map<Review>(createdReview);

            if (!await _reviewRepository.CreateReviewAsync(mappedReview))
            {
                ModelState.AddModelError("", "Something went wrong creating the reivew");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedReview);
        }

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto updatedReview)
        {
            if (updatedReview == null)
            {
                return BadRequest("A valid review was not sent.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updatedReview.ReviewId != null)
            {
                if (!await _reviewRepository.ReviewExistsAsync(updatedReview.ReviewId.Value)) 
                {
                    return NotFound($"No review with the Id of {updatedReview.ReviewId} was found.");
                }
            }
            else 
            {
                return BadRequest(ModelState);
            }

            if (reviewId != updatedReview.ReviewId) 
            {
                return BadRequest("Route reviewId does not match body reviewId");
            }

            if (!await _userRepository.UserExistsAsync(updatedReview.UserId))
            {
                return NotFound($"No user with the Id of {updatedReview.UserId} was found.");
            }

            if (!await _productRepository.ProductExistsAsync(updatedReview.ProductId))
            {
                return NotFound($"No product with the Id of {updatedReview.ProductId} was found.");
            }

            var mappedReview = _mapper.Map<Review>(updatedReview);

            if (!await _reviewRepository.UpdateReviewAsync(mappedReview))
            {
                ModelState.AddModelError("", "Something went wrong updating the review");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedReview);
        }


        [HttpDelete("{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExistsAsync(reviewId))
            {
                return NotFound($"No review with the Id of {reviewId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _reviewRepository.DeleteReviewAsync(reviewId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the reivew");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetAllReviewsFromAUser(int userId) 
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"No user with the Id of {userId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedreviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetAllReviewsFromAUserAsync(userId));

            if (mappedreviews == null) 
            {
                return Ok();
            }

            return Ok(mappedreviews);
        }

        [HttpGet("products/{productId}")]
        public async Task<IActionResult> GetAllReviewsOfAProduct(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return NotFound($"No product with the Id of {productId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedreviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetAllReviewsOfAProductAsync(productId));

            if (mappedreviews == null)
            {
                return Ok();
            }

            return Ok(mappedreviews);
        }
    }
}

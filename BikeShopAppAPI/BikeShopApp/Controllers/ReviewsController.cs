using AutoMapper;
using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using BikeShopApp.Core.Attributes;
using Microsoft.AspNetCore.Authorization;

namespace BikeShopApp.WebAPI.Controllers
{
    public class ReviewsController : CustomControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Review Controller Constructor.
        /// </summary>
        public ReviewsController(IReviewRepository reviewRepository, IUserRepository userRepository, IProductRepository productRepository, IMapper mapper)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the review with the passed Id.
        /// </summary>
        /// <param name="reviewId"></param>
        [HttpGet("{reviewId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetReview(int reviewId) 
        {
            if (!await _reviewRepository.ReviewExistsAsync(reviewId)) 
            {
                return Problem(detail: $"No review with the Id of {reviewId} was found.", statusCode: 404, title: "Not Found");
            }

            ReviewDto reviewDto = _mapper.Map<ReviewDto>(await _reviewRepository.GetReviewAsync(reviewId));

            if (reviewDto == null) 
            {
                return Problem(detail: "Something went wrong while getting the reivew.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(reviewDto);
        }

        /// <summary>
        /// Create a new review based on the passed reviewDto.
        /// </summary>
        /// <param name="reviewDto"></param>
        [HttpPost]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return Problem(detail: "A valid review was not passed.", statusCode: 400, title: "Bad Request");
            }

            if (!await _userRepository.UserExistsAsync(reviewDto.UserId.Value)) 
            {
                return Problem(detail: $"No user with the id of {reviewDto.UserId.Value} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.ProductExistsAsync(reviewDto.ProductId.Value))
            {
                return Problem(detail: $"No product with the id of {reviewDto.ProductId} was found.", statusCode: 404, title: "Not Found");
            }

            Review review = _mapper.Map<Review>(reviewDto);

            if (!await _reviewRepository.CreateReviewAsync(review))
            {
                return Problem(detail: "Something went wrong while creating the reivew.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(review);
        }

        /// <summary>
        /// Update an existing review based on the passed reviewDto.
        /// </summary>
        /// <param name="reviewId"></param>
        /// <param name="reviewDto"></param>
        [HttpPut("{reviewId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto reviewDto)
        {
            if (reviewDto == null)
            {
                return Problem(detail: "A valid review was not passed.", statusCode: 400, title: "Bad Request");
            }

            if (reviewDto.ReviewId != null)
            {
                if (!await _reviewRepository.ReviewExistsAsync(reviewDto.ReviewId.Value)) 
                {
                    return Problem(detail: $"No review with the Id of {reviewDto.ReviewId} was found.", statusCode: 404, title: "Not Found");
                }
            }
            else 
            {
                return Problem(detail: "A valid review id was not passed.", statusCode: 400, title: "Bad Request");
            }

            if (reviewId != reviewDto.ReviewId) 
            {
                return Problem(detail: "Route reviewId does not match body reviewId.", statusCode: 400, title: "Bad Request");
            }

            if (!await _userRepository.UserExistsAsync(reviewDto.UserId.Value))
            {
                return Problem(detail: $"No user with the Id of {reviewDto.UserId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.ProductExistsAsync(reviewDto.ProductId.Value))
            {
                return Problem(detail: $"No product with the Id of {reviewDto.ProductId} was found.", statusCode: 404, title: "Not Found");
            }

            Review review = _mapper.Map<Review>(reviewDto);

            if (!await _reviewRepository.UpdateReviewAsync(review))
            {
                return Problem(detail: "Something went wrong while updating the review.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(review);
        }

        /// <summary>
        /// Delete an existing review based on the passed Id.
        /// </summary>
        /// <param name="reviewId"></param>
        [HttpDelete("{reviewId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (!await _reviewRepository.ReviewExistsAsync(reviewId))
            {
                return Problem(detail: $"No review with the Id of {reviewId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _reviewRepository.DeleteReviewAsync(reviewId))
            {
                return Problem(detail: "Something went wrong while deleting the reivew.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Get all reviews from the user with the passed Id.
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("users/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllReviewsFromAUser(int userId) 
        {

            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"No user with the Id of {userId} was found.", statusCode: 404, title: "Not Found");
            }

            List<ReviewDto> reviewDtos = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetAllReviewsFromAUserAsync(userId));

            if (reviewDtos == null) 
            {
                return NoContent();
            }

            return Ok(reviewDtos);
        }

        /// <summary>
        /// Get all the reviews for the product with the passed Id.
        /// </summary>
        /// <param name="productId"></param>
        [HttpGet("products/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllReviewsOfAProduct(int productId)
        {
            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return Problem(detail: $"No product with the Id of {productId} was found.", statusCode: 404, title: "Not Found");
            }

            List<ReviewDto> reviewDtos = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetAllReviewsOfAProductAsync(productId));

            if (reviewDtos == null)
            {
                return NoContent();
            }

            return Ok(reviewDtos);
        }
    }
}

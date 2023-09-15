using AutoMapper;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using BikeShopApp.Core.Attributes;

namespace BikeShopApp.WebAPI.Controllers
{
    public class CartsController : CustomControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Carts Controller Constructor.
        /// </summary>
        public CartsController(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a cart using its Id.
        /// </summary>
        /// <param name="cartId"></param>
        [HttpGet("{cartId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetCart(int cartId) 
        {           
            if (!await _cartRepository.CartExistsAsync(cartId)) 
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", title: "Not Found", statusCode: 404);
            }

            CartDto cartDto = _mapper.Map<CartDto>(await _cartRepository.GetCartAsync(cartId));

            if (cartDto == null) 
            {
                return Problem(detail: "Something went wrong while getting the cart.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(cartDto);
        }

        /// <summary>
        /// Add a product to a user's cart using its Id.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="cartItemDto"></param>
        [HttpPost("{cartId}/products")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> AddToCart(int cartId, CartItemDto cartItemDto)
        {
            if (!await _cartRepository.CartExistsAsync(cartItemDto.CartId.Value))
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.ProductExistsAsync(cartItemDto.ProductId.Value))
            {                
                return Problem(detail: $"No product with the Id of {cartItemDto.ProductId} was found.", statusCode: 404, title: "Not Found");
            }

            if (cartId != cartItemDto.CartId) 
            {
                return Problem(detail: "Route cartId doesn't match body cartId.", statusCode: 400, title: "Bad Request");
            }

            if (!await _cartRepository.AddToCartAsync(cartItemDto.CartId.Value, cartItemDto.ProductId.Value))
            {
                return Problem(detail: "Something went wrong while adding the product to the cart.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Remove a product from a user's cart using its Id.
        /// </summary>
        /// <param name="cartId"></param>
        /// <param name="productId"></param>
        [HttpDelete("{cartId}/products/{productId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> RemoveProductFromCart(int cartId, int productId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return Problem(detail: $"No product with the Id of {productId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _cartRepository.RemoveProductFromCartAsync(cartId, productId))
            {
                return Problem(detail: "Something went wrong while removing the product from the cart.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Get all products in a user's cart.
        /// </summary>
        /// <param name="cartId"></param>
        [HttpGet("{cartId}/products")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetProductsInCart(int cartId) 
        {
            if (!await _cartRepository.CartExistsAsync(cartId)) 
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", statusCode: 404, title: "Not Found");
            }

            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(await _cartRepository.GetProductsInCartAsync(cartId));

            if (productDtos == null) 
            {
                return Problem(detail: "Something went wrong while getting the products from the cart.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDtos);
        }

        /// <summary>
        /// Get the total number of items in a user's cart
        /// </summary>
        /// <param name="cartId"></param>
        [HttpGet("{cartId}/totalitems")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetNumberOfItemsInCart(int cartId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", statusCode: 404, title: "Not Found");
            }

            var numOfItems = await _cartRepository.GetNumberOfItemsInCartAsync(cartId);

            if (numOfItems <= -1)
            {
                return Problem(detail: "Something went wrong while getting the number of items in the cart.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(numOfItems);
        }

        /// <summary>
        /// Get the total price of a user's cart.
        /// </summary>
        /// <param name="cartId"></param>
        [HttpGet("{cartId}/totalprice")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetTotalPriceOfCart(int cartId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", statusCode: 404, title: "Not Found");
            }

            var totalPrice = await _cartRepository.GetTotalPriceOfCartAsync(cartId);

            if (totalPrice <= -1)
            {
                return Problem(detail: "Something went wrong while getting the total price of the cart", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(totalPrice);
        }

        /// <summary>
        /// Remove all items in a user's cart.
        /// </summary>
        /// <param name="cartId"></param>
        [HttpDelete("{cartId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> EmptyCart(int cartId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return Problem(detail: $"No cart with the Id of {cartId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _cartRepository.EmptyCartAsync(cartId))
            {
                return Problem(detail: "Something went wrong while emptying the cart.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }
    }
}

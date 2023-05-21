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
    public class CartsController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CartsController(ICartRepository cartRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }


        [HttpGet("{cartId}")]
        public async Task<IActionResult> GetCart(int cartId) 
        {
            if (!await _cartRepository.CartExistsAsync(cartId)) 
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var cartMapped = _mapper.Map<CartDto>(await _cartRepository.GetCartAsync(cartId));

            if (cartMapped == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok(cartMapped);
        }

        [HttpPost("{cartId}/products")]
        public async Task<IActionResult> AddToCart(int cartId, [FromBody] CartItemDto cartItem)
        {
            if (!await _cartRepository.CartExistsAsync(cartItem.CartId))
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!await _productRepository.ProductExistsAsync(cartItem.ProductId))
            {
                return NotFound($"No product with the Id of {cartItem.ProductId} was found.");
            }

            if (cartId != cartItem.CartId) 
            {
                return BadRequest("Route cartId doesn't match body cartId");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _cartRepository.AddToCartAsync(cartItem.CartId, cartItem.ProductId))
            {
                ModelState.AddModelError("", "Something went wrong adding the product to the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        [HttpDelete("{cartId}/products/{productId}")]
        public async Task<IActionResult> RemoveProductFromCart(int cartId, int productId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!await _productRepository.ProductExistsAsync(productId))
            {
                return NotFound($"No product with the Id of {productId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _cartRepository.RemoveProductFromCartAsync(cartId, productId))
            {
                ModelState.AddModelError("", "Something went wrong removing the product from the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpGet("{cartId}/products")]
        public async Task<IActionResult> GetProductsInCart(int cartId) 
        {
            if (!await _cartRepository.CartExistsAsync(cartId)) 
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProducts = _mapper.Map<List<ProductDto>>(await _cartRepository.GetProductsInCartAsync(cartId));

            if (mappedProducts == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the products from the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProducts);
        }

        [HttpGet("{cartId}/totalitems")]
        public async Task<IActionResult> GetNumberOfItemsInCart(int cartId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var numOfItems = await _cartRepository.GetNumberOfItemsInCartAsync(cartId);

            if (numOfItems == -1)
            {
                ModelState.AddModelError("", "Something went wrong getting the number of items in the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok(numOfItems);
        }

        [HttpGet("{cartId}/totalprice")]
        public async Task<IActionResult> GetTotalPriceOfCart(int cartId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var totalPrice = await _cartRepository.GetTotalPriceOfCartAsync(cartId);

            if (totalPrice == -1)
            {
                ModelState.AddModelError("", "Something went wrong getting the total price of the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok(totalPrice);
        }


        [HttpDelete("{cartId}")]
        public async Task<IActionResult> EmptyCart(int cartId)
        {
            if (!await _cartRepository.CartExistsAsync(cartId))
            {
                return NotFound($"No cart with the Id of {cartId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _cartRepository.EmptyCartAsync(cartId))
            {
                ModelState.AddModelError("", "Something went wrong emptying the cart.");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }
    }
}

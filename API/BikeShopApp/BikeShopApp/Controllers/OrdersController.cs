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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IUserRepository userRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedOrders = _mapper.Map<List<OrderDto>>(await _orderRepository.GetOrdersAsync());

            if (mappedOrders == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the orders");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedOrders);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            if (!await _orderRepository.OrderExistsAsync(orderId)) 
            {
                return NotFound($"No Order with the Id of {orderId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedOrder = _mapper.Map<OrderDto>(await _orderRepository.GetOrderAsync(orderId));

            if (mappedOrder == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the Order");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedOrder);
        }


        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderAndProductsDto orderAndProducts)
        {
            if (orderAndProducts == null)
            {
                return BadRequest("No orderAndProducts was passed.");
            }

            if (orderAndProducts.Order == null || orderAndProducts.ProductsIds == null)
            {
                return BadRequest("No Order or products were passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedOrder = _mapper.Map<Order>(orderAndProducts.Order);

            if (!await _orderRepository.CreateOrderAsync(mappedOrder, orderAndProducts.ProductsIds))
            {
                ModelState.AddModelError("", "Something went wrong creating the Order");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }

        [HttpPut("{orderId}")]
        public async Task<IActionResult> CreateOrder(int orderId, [FromBody]OrderDto updatedOrder)
        {
            if (updatedOrder == null)
            {
                return BadRequest("No Order was passed.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (updatedOrder.OrderId != null)
            {
                if (!await _orderRepository.OrderExistsAsync(updatedOrder.OrderId.Value))
                {
                    return NotFound($"No Order with the Id of {updatedOrder.OrderId.Value} was found.");
                }
            }
            else 
            {
                return BadRequest(ModelState);
            }

            var mappedOrder = _mapper.Map<Order>(updatedOrder);

            if (!await _orderRepository.UpdateOrderAsync(mappedOrder))
            {
                ModelState.AddModelError("", "Something went wrong updating the Order");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        [HttpDelete("{orderId}")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!await _orderRepository.OrderExistsAsync(orderId))
            {
                return NotFound($"No Order with the Id of {orderId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!await _orderRepository.DeleteOrderAsync(orderId))
            {
                ModelState.AddModelError("", "Something went wrong deleting the Order");
                return StatusCode(500, ModelState);
            }

            return Ok();
        }


        [HttpGet("{orderId}/user")]
        public async Task<IActionResult> GetUserOfOrder(int orderId) 
        {
            if (!await _orderRepository.OrderExistsAsync(orderId)) 
            {
                return NotFound($"No Order with the Id of {orderId} was found.");
            }

            if (!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var mappedUser = _mapper.Map<UserDto>(await _orderRepository.GetUserOfOrderAsync(orderId));

            if (mappedUser == null) 
            {
                ModelState.AddModelError("", "Something went wrong getting the user");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedUser);
        }


        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetOrdersFromUser(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"No user with the Id of {userId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedOrders = _mapper.Map<List<OrderDto>>(await _orderRepository.GetOrdersFromUserAsync(userId));

            if (mappedOrders == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the orders");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedOrders);
        }

        [HttpGet("users/{userId}/pages")]
        public async Task<IActionResult> GetOrdersFromUserByPage(int userId, [FromQuery] string currentPage, string pageResults)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return NotFound($"No user with the Id of {userId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var ordersResponse = await _orderRepository.GetOrdersFromUserByPageAsync(userId, currentPage, pageResults);

            if (ordersResponse == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the orders by page");
                return StatusCode(500, ModelState);
            }

            return Ok(ordersResponse);
        }


        [HttpGet("{orderId}/products")]
        public async Task<IActionResult> GetAllProductsInOrder(int orderId)
        {
            if (!await _orderRepository.OrderExistsAsync(orderId))
            {
                return NotFound($"No Order with the Id of {orderId} was found.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedProducts = _mapper.Map<List<ProductDto>>(await _orderRepository.GetAllProductsInOrderAsync(orderId));

            if (mappedProducts == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the products");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedProducts);
        }


        [HttpGet("period")]
        public async Task<IActionResult> GetAllOrdersFromAfterPeriod([FromQuery] DateTime date)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var mappedOrders = _mapper.Map<List<OrderDto>>(await _orderRepository.GetAllOrdersFromAfterPeriodAsync(date));

            if (mappedOrders == null)
            {
                ModelState.AddModelError("", "Something went wrong getting the orders");
                return StatusCode(500, ModelState);
            }

            return Ok(mappedOrders);
        }
    }
}

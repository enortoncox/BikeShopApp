using AutoMapper;
using BikeShopApp.Core.Models;
using BikeShopApp.Core.DTO;
using BikeShopApp.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Mvc;
using BikeShopApp.Core.Attributes;

namespace BikeShopApp.WebAPI.Controllers
{
    public class OrdersController : CustomControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Orders Controller Constructor.
        /// </summary>
        public OrdersController(IOrderRepository orderRepository, IUserRepository userRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all the orders of every user.
        /// </summary>
        [HttpGet]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetOrders()
        {
            List<OrderDto> orderDtos = _mapper.Map<List<OrderDto>>(await _orderRepository.GetOrdersAsync());

            if (orderDtos == null) 
            {
                return Problem(detail: "Something went wrong while getting the orders.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(orderDtos);
        }

        /// <summary>
        /// Get the order with the passed Id.
        /// </summary>
        /// <param name="orderId"></param>
        [HttpGet("{orderId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetOrder(int orderId)
        {
            if (!await _orderRepository.OrderExistsAsync(orderId)) 
            {
                return Problem(detail: $"No Order with the Id of {orderId} was found.", statusCode: 404, title: "Not Found");
            }

            OrderDto orderDto = _mapper.Map<OrderDto>(await _orderRepository.GetOrderAsync(orderId));

            if (orderDto == null)
            {
                return Problem(detail: "Something went wrong while getting the order.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(orderDto);
        }

        /// <summary>
        /// Create a new order based on the orderAndProductDto.
        /// </summary>
        /// <param name="orderAndProductsDto"></param>
        [HttpPost]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> CreateOrder(OrderAndProductsDto orderAndProductsDto)
        {
            if (orderAndProductsDto == null)
            {
                return Problem(detail: "No orderAndProducts was passed.", statusCode: 400, title: "Bad Request");
            }

            if (orderAndProductsDto.Order == null || orderAndProductsDto.ProductsIds == null)
            {
                return Problem(detail: "No orders or product ids were passed.", statusCode: 400, title: "Bad Request");
            }

            Order order = _mapper.Map<Order>(orderAndProductsDto.Order);

            if (!await _orderRepository.CreateOrderAsync(order, orderAndProductsDto.ProductsIds))
            {
                return Problem(detail: "Something went wrong while creating the order.", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Update an order based on the passed orderDto.
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="orderDto"></param>
        [HttpPut("{orderId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> UpdateOrder(int orderId, OrderDto orderDto)
        {
            if (orderDto == null)
            {
                return Problem(detail: "No Order was passed.", statusCode: 400, title: "Bad Request");
            }

            if (orderDto.OrderId != null)
            {
                if (!await _orderRepository.OrderExistsAsync(orderDto.OrderId.Value))
                {
                    return Problem(detail: $"No Order with the Id of {orderDto.OrderId.Value} was found.", statusCode: 404, title: "Not Found");
                }
            }
            else 
            {
                return Problem(detail: "No Order Id was passed.", statusCode: 400, title: "Bad Request");
            }

            Order order = _mapper.Map<Order>(orderDto);

            if (!await _orderRepository.UpdateOrderAsync(order))
            {
                return Problem(detail: "Something went wrong while updating the order", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Delete the order with the passed Id.
        /// </summary>
        /// <param name="orderId"></param>
        [HttpDelete("{orderId}")]
        [CustomAuthorize(roles: "Admin")]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            if (!await _orderRepository.OrderExistsAsync(orderId))
            {
                return Problem(detail: $"No Order with the Id of {orderId} was found.", statusCode: 404, title: "Not Found");
            }

            if (!await _orderRepository.DeleteOrderAsync(orderId))
            {
                return Problem(detail: "Something went wrong while deleting the order", statusCode: 500, title: "Internal Server Error");
            }

            return NoContent();
        }

        /// <summary>
        /// Get the user of the order with the passed Id.
        /// </summary>
        /// <param name="orderId"></param>
        [HttpGet("{orderId}/user")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetUserOfOrder(int orderId) 
        {
            if (!await _orderRepository.OrderExistsAsync(orderId)) 
            {
                return Problem(detail: $"No Order with the Id of {orderId} was found.", statusCode: 404, title: "Not Found");
            }

            UserDto userDto = _mapper.Map<UserDto>(await _orderRepository.GetUserOfOrderAsync(orderId));

            if (userDto == null) 
            {
                return Problem(detail: "Something went wrong while getting the user.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(userDto);
        }

        /// <summary>
        /// Get all orders from the user with the passed Id.
        /// </summary>
        /// <param name="userId"></param>
        [HttpGet("users/{userId}")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetOrdersFromUser(int userId)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"No user with the Id of {userId} was found.", statusCode: 404, title: "Not Found");
            }

            List<OrderDto> orderDtos = _mapper.Map<List<OrderDto>>(await _orderRepository.GetOrdersFromUserAsync(userId));

            if (orderDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the orders.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(orderDtos);
        }

        /// <summary>
        /// Get all orders of a user for the current page.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageResults"></param>
        [HttpGet("users/{userId}/pages")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetOrdersFromUserByPage(int userId, [FromQuery] string currentPage, string pageResults)
        {
            if (!await _userRepository.UserExistsAsync(userId))
            {
                return Problem(detail: $"No user with the Id of {userId} was found.", statusCode: 404, title: "Not Found");
            }

            OrdersPageResponseDto? ordersPageResponse = await _orderRepository.GetOrdersFromUserByPageAsync(userId, currentPage, pageResults);

            if (ordersPageResponse == null)
            {
                return Problem(detail: "Something went wrong while getting the orders by page.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(ordersPageResponse);
        }

        /// <summary>
        /// Get all of the products in the order with the passed Id.
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpGet("{orderId}/products")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetAllProductsInOrder(int orderId)
        {
            if (!await _orderRepository.OrderExistsAsync(orderId))
            {
                return Problem(detail: $"No Order with the Id of {orderId} was found.", statusCode: 404, title: "Not Found");
            }

            List<ProductDto> productDtos = _mapper.Map<List<ProductDto>>(await _orderRepository.GetAllProductsInOrderAsync(orderId));

            if (productDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the products.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(productDtos);
        }

        /// <summary>
        /// Get all orders from after the passed date.
        /// </summary>
        /// <param name="date"></param>
        [HttpGet("period")]
        [CustomAuthorize(roles: "User")]
        public async Task<IActionResult> GetAllOrdersFromAfterPeriod([FromQuery] DateTime date)
        {
            List<OrderDto> orderDtos = _mapper.Map<List<OrderDto>>(await _orderRepository.GetAllOrdersFromAfterPeriodAsync(date));

            if (orderDtos == null)
            {
                return Problem(detail: "Something went wrong while getting the orders.", statusCode: 500, title: "Internal Server Error");
            }

            return Ok(orderDtos);
        }
    }
}

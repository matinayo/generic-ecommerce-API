using AutoMapper.Features;
using HalceraAPI.Models;
using HalceraAPI.Models.Enums;
using HalceraAPI.Models.Requests.OrderHeader;
using HalceraAPI.Models.Requests.Product;
using HalceraAPI.Services.Contract;
using HalceraAPI.Services.Operations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace HalceraAPI.Areas.Admin.Controllers
{
    [Authorize]
    [Area("Admin")]
    [Route("api/[area]/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IAdminOrderOperation _orderOperation;

        public OrdersController(IAdminOrderOperation orderOperation)
        {
            _orderOperation = orderOperation;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderResponse>>> GetOrdersAsync(OrderStatus? orderStatus)
        {
            try
            {
                IEnumerable<OrderResponse>? listOfOrders = await _orderOperation.GetOrdersAsync(orderStatus);

                return Ok(listOfOrders);
            }
            catch (Exception exception)
            {
                return BadRequest(
                    Problem(
                        statusCode: StatusCodes.Status400BadRequest,
                        detail: exception?.InnerException?.Message ?? exception?.Message));
            }
        }
    }
}

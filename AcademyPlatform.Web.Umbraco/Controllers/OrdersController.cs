namespace AcademyPlatform.Web.Umbraco.Controllers
{
    using System.Web.Mvc;

    using AcademyPlatform.Models.Payments;
    using AcademyPlatform.Services.Contracts;

    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrdersService _ordersService;

        public OrdersController(IOrdersService ordersService)
        {
            _ordersService = ordersService;
        }

        [HttpGet]
        public ActionResult Order(int orderId)
        {
            Order order = _ordersService.GetById(orderId);
            if (order.User.Username != User.Identity.Name)
            {
                return HttpNotFound();
            }

            return View(order);

        }
    }
}
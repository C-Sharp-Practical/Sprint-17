using System.Linq;
using EFC.Models;
using EFC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EFC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly ISupermarketService _supermarketService;
        private readonly IProductService _productService;

        public OrdersController(
            IOrderService orderService,
            ICustomerService customerService,
            ISupermarketService supermarketService,
            IProductService productService)
        {
            _orderService = orderService;
            _customerService = customerService;
            _supermarketService = supermarketService;
            _productService = productService;
        }

        // GET: Orders
        public async Task<IActionResult> Index(int? id)
        {
            var viewModel = new OrderIndexData();

            viewModel.Orders = (await _orderService.GetAllAsync()).ToList();

            if (id.HasValue)
            {
                ViewData["OrderID"] = id.Value;

                var order = await _orderService.GetByIdAsync(id.Value);
                viewModel.OrderDetails = order?.OrderDetails ?? Enumerable.Empty<OrderDetail>();
            }

            return View(viewModel);
        }

        // GET: Orders/Create
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewData["CustomerId"] = new SelectList((await _customerService.GetAllAsync()).Select(c => new { c.Id, c.LastName }), "Id", "LastName");
            ViewData["SuperMarketId"] = new SelectList((await _supermarketService.GetAllAsync()).Select(s => new { s.Id, s.Name }), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order model)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateAsync(model);
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList((await _customerService.GetAllAsync()).Select(c => new { c.Id, c.LastName }), "Id", "LastName", model.CustomerId);
            ViewData["SuperMarketId"] = new SelectList((await _supermarketService.GetAllAsync()).Select(s => new { s.Id, s.Name }), "Id", "Name", model.SuperMarketId);
            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var order = await _orderService.GetByIdAsync(id.Value);

            if (order == null) return NotFound();

            ViewData["CustomerId"] = new SelectList((await _customerService.GetAllAsync()).Select(c => new { c.Id, c.LastName }), "Id", "LastName", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList((await _supermarketService.GetAllAsync()).Select(s => new { s.Id, s.Name }), "Id", "Name", order.SuperMarketId);

            // Завантажуємо продукти для випадаючого списку "Додати товар"
            ViewData["ProductId"] = new SelectList((await _productService.GetAllAsync()).Select(p => new { p.Id, p.Name }), "Id", "Name");

            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order order)
        {
            if (id != order.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _orderService.UpdateAsync(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await _orderService.GetByIdAsync(order.Id) == null) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustomerId"] = new SelectList((await _customerService.GetAllAsync()).Select(c => new { c.Id, c.LastName }), "Id", "LastName", order.CustomerId);
            ViewData["SuperMarketId"] = new SelectList((await _supermarketService.GetAllAsync()).Select(s => new { s.Id, s.Name }), "Id", "Name", order.SuperMarketId);
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddDetail(int OrderId, int ProductId, double Quantity)
        {
            var detail = new OrderDetail
            {
                OrderId = OrderId,
                ProductId = ProductId,
                Quantity = Quantity
            };

            if (OrderId > 0 && ProductId > 0 && Quantity > 0)
            {
                await _orderService.AddDetailAsync(detail);
            }

            return RedirectToAction(nameof(Edit), new { id = OrderId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetail(int id)
        {
            var detailEntity = await _orderService.GetDetailByIdAsync(id);
            int orderId = 0;
            if (detailEntity != null)
            {
                orderId = detailEntity.OrderId;
            }

            await _orderService.DeleteDetailAsync(id);
            return RedirectToAction(nameof(Edit), new { id = orderId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _orderService.GetByIdAsync(id.Value);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
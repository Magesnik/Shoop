using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Data;
using OnlineStore.Models;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICartService _cartService;

        public OrdersController(ApplicationDbContext context, ICartService cartService)
        {
            _context = context;
            _cartService = cartService;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            var orders = await _context.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
            return View(orders);
        }

        // GET: Orders/Checkout
        public async Task<IActionResult> Checkout()
        {
            var sessionId = GetSessionId();
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var cartItems = await _cartService.GetCartItemsAsync(sessionId);
            if (!cartItems.Any())
            {
                TempData["Error"] = "Количката е празна.";
                return RedirectToAction("Index", "Cart");
            }

            var cartTotal = await _cartService.GetCartTotalAsync(sessionId);
            ViewBag.CartTotal = cartTotal;
            ViewBag.CartItems = cartItems;

            return View(new Order());
        }

        // POST: Orders/Checkout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(Order order)
        {
            var sessionId = GetSessionId();
            if (string.IsNullOrEmpty(sessionId))
            {
                return RedirectToAction("Index", "Cart");
            }

            var cartItems = await _cartService.GetCartItemsAsync(sessionId);
            if (!cartItems.Any())
            {
                TempData["Error"] = "Количката е празна.";
                return RedirectToAction("Index", "Cart");
            }

            if (ModelState.IsValid)
            {
                order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}";
                order.TotalAmount = await _cartService.GetCartTotalAsync(sessionId);
                order.OrderDate = DateTime.Now;
                order.Status = OrderStatus.Pending;

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Изчистване на количката
                await _cartService.ClearCartAsync(sessionId);

                TempData["Success"] = $"Поръчката {order.OrderNumber} беше създадена успешно!";
                return RedirectToAction("OrderConfirmation", new { id = order.Id });
            }

            // Ако има грешки, покажи отново формата
            var cartTotal = await _cartService.GetCartTotalAsync(sessionId);
            ViewBag.CartTotal = cartTotal;
            ViewBag.CartItems = cartItems;

            return View(order);
        }

        // GET: Orders/OrderConfirmation/5
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Orders/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        private string? GetSessionId()
        {
            return HttpContext.Session.GetString("CartSessionId");
        }
    }
}
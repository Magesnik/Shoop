using Microsoft.AspNetCore.Mvc;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        // GET: Cart
        public async Task<IActionResult> Index()
        {
            var sessionId = GetOrCreateSessionId();
            var cartItems = await _cartService.GetCartItemsAsync(sessionId);
            var cartTotal = await _cartService.GetCartTotalAsync(sessionId);

            ViewBag.CartTotal = cartTotal;
            return View(cartItems);
        }

        // POST: Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _productService.GetProductByIdAsync(productId);
            if (product == null || product.StockQuantity < quantity)
            {
                TempData["Error"] = "Продуктът не е наличен или няма достатъчно количество на склад.";
                return RedirectToAction("Details", "Products", new { id = productId });
            }

            var sessionId = GetOrCreateSessionId();
            await _cartService.AddToCartAsync(productId, quantity, sessionId);

            TempData["Success"] = $"{product.Name} беше добавен в количката.";
            return RedirectToAction("Index");
        }

        // POST: Cart/UpdateQuantity
        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _cartService.UpdateCartItemAsync(cartItemId, quantity);
            return RedirectToAction("Index");
        }

        // POST: Cart/Remove
        [HttpPost]
        public async Task<IActionResult> Remove(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            TempData["Success"] = "Продуктът беше премахнат от количката.";
            return RedirectToAction("Index");
        }

        // POST: Cart/Clear
        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            var sessionId = GetOrCreateSessionId();
            await _cartService.ClearCartAsync(sessionId);
            TempData["Success"] = "Количката беше изчистена.";
            return RedirectToAction("Index");
        }

        // GET: Cart/GetCartCount (Ajax)
        public async Task<IActionResult> GetCartCount()
        {
            var sessionId = GetOrCreateSessionId();
            var count = await _cartService.GetCartItemCountAsync(sessionId);
            return Json(new { count });
        }

        private string GetOrCreateSessionId()
        {
            var sessionId = HttpContext.Session.GetString("CartSessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("CartSessionId", sessionId);
            }
            return sessionId;
        }
    }
}
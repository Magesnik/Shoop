using Microsoft.AspNetCore.Mvc;
using OnlineStore.Services;

namespace OnlineStore.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET: Products
        public async Task<IActionResult> Index(string category, string search)
        {
            ViewBag.Categories = await _productService.GetCategoriesAsync();
            ViewBag.CurrentCategory = category;
            ViewBag.SearchTerm = search;

            IEnumerable<OnlineStore.Models.Product> products;

            if (!string.IsNullOrEmpty(search))
            {
                products = await _productService.SearchProductsAsync(search);
            }
            else if (!string.IsNullOrEmpty(category))
            {
                products = await _productService.GetProductsByCategoryAsync(category);
            }
            else
            {
                products = await _productService.GetAllProductsAsync();
            }

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            // Увеличаване на броя прегледи
            await _productService.UpdateProductViewCountAsync(id);

            // Взимане на свързани продукти от същата категория
            var relatedProducts = await _productService.GetProductsByCategoryAsync(product.Category);
            ViewBag.RelatedProducts = relatedProducts.Where(p => p.Id != id).Take(4);

            return View(product);
        }

        // GET: Products/Popular
        public async Task<IActionResult> Popular()
        {
            var popularProducts = await _productService.GetPopularProductsAsync(20);
            return View(popularProducts);
        }
    }
}
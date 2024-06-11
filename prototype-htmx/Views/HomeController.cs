using htmx.Models;
using Htmx;
using htmx_prototype.Data;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace prototype_htmx.Controllers
{
    public class ProductFormModel
    {
        public string productName { get; set; } = string.Empty;
        public double productPrice { get; set; }
        public string productDescription { get; set; } = string.Empty;
        public string productImage { get; set; } = string.Empty;
    }

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected HtmxDbContext db;
        public HomeController(ILogger<HomeController> logger, HtmxDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        [HttpPost("/add-product")]
        public async Task<IActionResult> Add([FromForm]ProductFormModel product)
        {
            var newProduct = new Product() { 
                Name = product.productName, 
                Description = product.productDescription, 
                Price = product.productPrice, 
                PreviewImage = product.productImage };

            db.Products.Add(newProduct);
            db.SaveChanges();
            var products = db.Products.ToList();

            return View("RefreshProducts", products);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var product = db.Products.FirstOrDefault(s=>s.Id == Id);
           
            if (product == null) return View("Error");

            db.Products.Remove(product);
            db.SaveChanges();

            var products = db.Products.ToList();
            return View("RefreshProducts",products);
        }
    }
}
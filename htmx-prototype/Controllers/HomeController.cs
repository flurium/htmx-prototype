using Htmx;
using htmx_prototype.Data;
using htmx_prototype.Models;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;
using System.Linq;

namespace htmx_prototype.Controllers
{
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
            var products = db.Products.OrderBy(p => p.Name).Take(6).ToList();
            bool hasMore = db.Products.Count() > products.Count;
            var model = new LoadMoreModel { Products = products, HasMore = hasMore };
            return View(model);
        }

        public record class ProductFormModel(string productName, double productPrice, string productDescription, IFormFile productImage);

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductFormModel product)
        {
            string productImageName = Guid.NewGuid().ToString();

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "Images", productImageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await product.productImage.CopyToAsync(stream);
            }

            var newProduct = new Product()
            {
                Name = product.productName,
                Description = product.productDescription,
                Price = product.productPrice,
                PreviewImage = filePath
            };

            db.Products.Add(newProduct);
            db.SaveChanges();

            var products = db.Products.OrderBy(p => p.Name).Take(6).ToList();
            bool hasMore = db.Products.Count() > products.Count;
            var model = new LoadMoreModel { Products = products, HasMore = hasMore };
            return View("_Products", model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var product = db.Products.FirstOrDefault(s => s.Id == Id);

            if (product == null) return View("Error");

            try
            {
                if (File.Exists(product.PreviewImage))
                {
                    File.Delete(product.PreviewImage);
                }
                else
                {
                    return View("Error");
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public IActionResult LoadMore(string cursor)
        {
            var products = db.Products
                             .Where(p => string.Compare(p.Id, cursor) > 0)
                             .OrderBy(p => p.Name)
                             .Take(6)
                             .ToList();

            var last = products.LastOrDefault();
            bool hasMore = (last != null) && db.Products.Any(p => string.Compare(p.Id, last.Id) > 0);

            return PartialView("_Products", new LoadMoreModel { Products = products, HasMore = hasMore });
        }

        [HttpPost]
        public async Task<IActionResult> EditName(string Id, string editName)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            product.Name = editName;
            db.Products.Update(product);
            db.SaveChanges();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditPrice(string Id, double editPrice)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            product.Price = editPrice;
            db.Products.Update(product);
            db.SaveChanges();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(string Id, string editImage)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            product.PreviewImage = editImage;
            db.Products.Update(product);
            db.SaveChanges();

            string content = "\r\n    <img class=\"image\" src=\"" + product.PreviewImage + "\" alt=\"" + product.Name + "\" />";
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> EditDescription(string Id, string editDescription)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            product.Description = editDescription;
            db.Products.Update(product);
            db.SaveChanges();

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Modal(string Id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            return PartialView("EditModal", product);
        }

        [HttpGet]
        public IActionResult CloseModal()
        {
            var products = db.Products.OrderBy(p => p.Name).Take(6).ToList();
            bool hasMore = db.Products.Count() > products.Count;
            var model = new LoadMoreModel { Products = products, HasMore = hasMore };
            return View("_Products", model);
        }

        [HttpGet]
        public IActionResult Details(string Id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            return View("Details", product);
        }
    }
}
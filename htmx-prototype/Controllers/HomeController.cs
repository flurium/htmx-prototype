using htmx_prototype.Data;
using htmx_prototype.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting.Internal;
using System;
using System.IO;

namespace htmx_prototype.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected HtmxDbContext db;
        protected IWebHostEnvironment environment;
        public HomeController(ILogger<HomeController> logger, HtmxDbContext context, IWebHostEnvironment environment)
        {
            _logger = logger;
            db = context;
            this.environment = environment;
        }

        public IActionResult Index()
        {
            var products = db.Products.OrderBy(p => p.Name.ToLower()).Take(6).ToList();
            bool hasMore = db.Products.Count() > products.Count;
            var model = new LoadMoreModel { Products = products, HasMore = hasMore };
            return View(model);
        }

        public record class ProductFormModel(string productName, IFormFile productImage, double productPrice, string productDescription);

        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductFormModel product)
        {
            string productImageName = Guid.NewGuid().ToString()+System.IO.Path.GetExtension(product.productImage.FileName);

            var filePath = Path.Combine(environment.WebRootPath, "Images", productImageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
              await product.productImage.CopyToAsync(stream);
            }

            var newProduct = new Product()
            {
                Name = product.productName,
                Description = product.productDescription,
                Price = product.productPrice,
                PreviewImage = "./Images/"+productImageName
            };

            db.Products.Add(newProduct);
            db.SaveChanges();

            var products = db.Products.OrderBy(p => p.Name.ToLower()).Take(6).ToList();
            bool hasMore = db.Products.Count() > products.Count;
            var model = new LoadMoreModel { Products = products, HasMore = hasMore };
            return View("_Products", model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var product = db.Products.FirstOrDefault(s => s.Id == Id);

            if (product == null) return View("Error");

            db.Products.Remove(product);
            db.SaveChanges();
            try
            {
                if (System.IO.File.Exists(environment.WebRootPath + product.PreviewImage))
                {
                    System.IO.File.Delete(environment.WebRootPath + product.PreviewImage);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult LoadMore(string cursor)
        {
            var products = db.Products
                             .OrderBy(p => p.Name.ToLower())
                             .Where(p => string.Compare(p.Id, cursor) > 0)
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
        public async Task<IActionResult> EditImage(string Id, IFormFile editImage)
        {
            if (editImage == null) return View("Error");
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            try
            {
                if (System.IO.File.Exists(environment.WebRootPath + product.PreviewImage))
                {
                    System.IO.File.Delete(environment.WebRootPath + product.PreviewImage);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return View("Error");
            }
            string productImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(editImage.FileName);

            var filePath = Path.Combine(environment.WebRootPath, "Images", productImageName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await editImage.CopyToAsync(stream);
            }

            product.PreviewImage = "./Images/" + productImageName;
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
            var products = db.Products.OrderBy(p => p.Name.ToLower()).Take(6).ToList();
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
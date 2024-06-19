using htmx_prototype.Data;
using htmx_prototype.Extensions;
using htmx_prototype.Models;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<IActionResult> Index()
        {
            var model = await ProductsModel();
            return View(model);
        }

        public record class ProductFormModel(string productName, IFormFile productImage, double productPrice, string productDescription);
        [HttpPost]
        public async Task<IActionResult> Add([FromForm] ProductFormModel product)
        {
            var newProduct = new Product()
            {
                Name = product.productName,
                Description = product.productDescription,
                Price = product.productPrice,
                PreviewImage = await SaveImage(product.productImage)
            };
            db.Products.Add(newProduct);
            db.SaveChanges();
            var model = await ProductsModel();
            return Partials.LoadMorePartial.Result(this,model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string Id)
        {
            var product = db.Products.FirstOrDefault(s => s.Id == Id);
            if (product == null) return View("Error");
            db.Products.Remove(product);
            db.SaveChanges();
            return await SafeDeleteImage(product.PreviewImage);
        }

        [HttpGet]
        public IActionResult LoadMore(string cursor)
        {
            var products = db.Products
                             .OrderBy(p => p.Id)
                             .Where(p => string.Compare(p.Id, cursor) > 0)
                             .Take(6)
                             .ToList();
            var last = products.LastOrDefault();
            bool hasMore = (last != null) && db.Products.Any(p => string.Compare(p.Id, last.Id) > 0);
            return Partials.LoadMorePartial.Result(this, new LoadMoreModel { Products = products, HasMore = hasMore });
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
            var result = await SafeDeleteImage(product.PreviewImage);
            if (result.GetType() != typeof(OkResult)) return result;
            product.PreviewImage = await SaveImage(editImage);
            db.Products.Update(product);
            db.SaveChanges();
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
            return Partials.EditModalPartial.Result(this, product);
        }

        [HttpGet]
        public async Task<IActionResult> CloseModal(string Id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            return Partials.ProductPartial.Result(this, product);
        }

        [HttpGet]
        public IActionResult Details(string Id)
        {
            var product = db.Products.FirstOrDefault(p => p.Id == Id);
            if (product == null) return View("Error");
            return Partials.DetailsPartial.Result(this, product);
        }

        private async Task<string> SaveImage(IFormFile image)
        {
            string productImageName = Guid.NewGuid().ToString() + System.IO.Path.GetExtension(image.FileName);
            var filePath = Path.Combine(environment.WebRootPath, "Images", productImageName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            return "./Images/" + productImageName;
        }

        private async Task<LoadMoreModel> ProductsModel()
        {
            var products = db.Products.OrderBy(p => p.Id).Take(6).ToList();
            bool hasMore = db.Products.Count() > products.Count;
            return new LoadMoreModel { Products = products, HasMore = hasMore };
        }
        private async Task<IActionResult> SafeDeleteImage(string name)
        {
            try
            {
                if (System.IO.File.Exists(environment.WebRootPath + name))
                {
                    System.IO.File.Delete(environment.WebRootPath + name);
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
    }
}
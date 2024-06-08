using Htmx;
using htmx_prototype.Data;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
namespace prototype_htmx.Pages
{
    public class ProductFormModel
    {
        public string productName { get; set; }
        public string productAmount { get; set; }
        public string productDescription { get; set; }
        public string productImage { get; set; }
    }

    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        protected HtmxDbContext db;
        public IndexModel(ILogger<IndexModel> logger, HtmxDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult OnGet()
        {
            return Request.IsHtmx() ? Content("<span>changed</span>", "text/html"):Page();
        }

        public IActionResult OnPostAdd([FromForm]ProductFormModel product)
        {

            return Request.IsHtmx() ? Content("<span>changed</span>", "text/html") : Page();
        }
    }
}
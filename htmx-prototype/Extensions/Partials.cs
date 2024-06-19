using htmx_prototype.Models;

namespace htmx_prototype.Extensions
{
    public static class Partials
    {
        public static readonly Partial<Product> ProductPartial = new("Product");
        public static readonly Partial<Product> DetailsPartial = new("Details");
        public static readonly Partial<Product> EditModalPartial = new("EditModal");
        public static readonly Partial<LoadMoreModel> LoadMorePartial = new("_Products");
        public static readonly Partial ProductFormPartial = new("_CreateForm");
    }
}

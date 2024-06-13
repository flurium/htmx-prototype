namespace htmx_prototype.Models
{
    public class LoadMoreModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public bool HasMore { get; set; }
    }
}
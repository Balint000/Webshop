namespace Webshop.Api.DTOs
{
    public class ProductQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search {  get; set; }
        public decimal? minPrice { get; set; }
        public decimal? maxPrice { get; set; }
        public string? Sortby { get; set; }
        public bool Descending { get; set; } = false;

    }
}

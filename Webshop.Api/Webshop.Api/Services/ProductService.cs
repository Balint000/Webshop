using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Webshop.Api.Data;
using Webshop.Api.DTOs;
using Webshop.Api.Models;
using Webshop.Api.Services.Interfaces;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace Webshop.Api.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductService> _logger;
        private readonly IDistributedCache _cache;

        private const string ProductsCacheKey = "products:list";
        private const string ProductByIdCacheKeyPrefix = "products:id";

        public ProductService(AppDbContext context, IMapper mapper, ILogger<ProductService> logger, IDistributedCache cache)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _cache = cache;
        }

        public async Task<(IEnumerable<ProductDto> Data, int TotalCount)> GetAllProductsAsync(ProductQueryParameters query)
        {
            var cacheKey = $"{ProductsCacheKey}:page={query.Page}:size={query.PageSize}:search={query.Search}:min={query.minPrice}:max={query.maxPrice}:sort={query.Sortby}:desc={query.Descending}";

            var cachedData = await _cache.GetStringAsync(cacheKey);

            if(cachedData != null)
            {
                _logger.LogInformation("Terméklista betöltve a Redis gyorsítótárból. Kulcs: {cacheKey}", cacheKey);
                var cachedResult = JsonSerializer.Deserialize<ProductListResponse>(cachedData);

                return (cachedResult!.Data, cachedResult.TotalCount);
            }

            _logger.LogInformation("A terméklista nem található a Redis gyorsítótárban. Betöltés az adatbázisból. Kulcs: {cacheKey}", cacheKey);

            var productsQuery = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                productsQuery = productsQuery.Where(p => p.Name.Contains(query.Search));
            }

            if (query.minPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price >= query.minPrice.Value);
            }

            if (query.maxPrice.HasValue)
            {
                productsQuery = productsQuery.Where(p => p.Price <= query.maxPrice.Value);
            }

            var totalCount = await productsQuery.CountAsync();

            productsQuery = query.Sortby?.ToLower() switch
            {
                "price" => query.Descending
                    ? productsQuery.OrderByDescending(p => p.Price) 
                    : productsQuery.OrderBy(p => p.Price),

                "name" => query.Descending
                    ? productsQuery.OrderByDescending(p => p.Name)
                    : productsQuery.OrderBy(p => p.Name),

                _ => productsQuery.OrderBy(p => p.Id)
            };

            var products = await productsQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            _logger.LogInformation("Termékek lekérése lapozással.");
            var data = _mapper.Map<IEnumerable<ProductDto>>(products);

            var response = new ProductListResponse
            {
                Data = data,
                TotalCount = totalCount
            };

            var serialized = JsonSerializer.Serialize(response);
            await _cache.SetStringAsync(
                cacheKey,
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(3),
                });

            _logger.LogInformation("Products saved to Redis cache. Key: {cacheKey}", cacheKey);

            return (response.Data, response.TotalCount);
        }

        public async Task<ProductDto?> GetProductByIdAsync(int id)
        {
            var cacheKey = $"product:{id}";

            var cachedData = await _cache.GetStringAsync(cacheKey);

            if (cachedData != null)
            {
                _logger.LogInformation("A(z) {ProductId} azonosítójú termék betöltve a Redis gyorsítótárból.", id);
                return JsonSerializer.Deserialize<ProductDto>(cachedData);
            }

            _logger.LogInformation("A(z) {ProductId} azonosítójú termék nem található a Redis gyorsítótárban. Betöltés az adatbázisból.", id);

            var product = await _context.Products.FindAsync(id);

            if (product == null) 
            {
                return null;
            }

            var result = _mapper.Map<ProductDto>(product);
            var serialized = JsonSerializer.Serialize(result);
            await _cache.SetStringAsync(
                cacheKey,
                serialized,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
                }
                );

            return result;
        }

        public async Task<ProductDto> CreateProductAsync(CreateProductDto dto)
        {
            _logger.LogInformation("Termék létrehozása ezzel a névvel: {ProductName}", dto.Name);
            var product = _mapper.Map<Product>(dto);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            await RemoveProductCaches(product.Id);

            _logger.LogInformation("A termék sikeresen létrehozva, azonosító: {ProductID}", product.Id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<ProductDto?> UpdateProductAsync(int id, CreateProductDto dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning("A termék frissítése sikertelen volt. A {ProductID} azonosítójú termék nem található.", id);
                return null;
            }

            _mapper.Map(dto, product);
            await _context.SaveChangesAsync();

            await RemoveProductCaches(id);

            return _mapper.Map<ProductDto>(product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                _logger.LogWarning("A termék törlése sikertelen volt. A {ProductID} azonosítójú termék nem található.", id);
                return false;
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            await RemoveProductCaches(id);

            return true;
        }

        private async Task RemoveProductCaches(int? productId = null)
        {
            if (productId.HasValue)
            {
                await _cache.RemoveAsync($"product:{productId.Value}");
            }

            _logger.LogInformation("A termék gyorsítótár érvénytelen: {ProductID}", productId);
        }
    }
}
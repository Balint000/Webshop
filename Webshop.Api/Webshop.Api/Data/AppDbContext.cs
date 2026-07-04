using Microsoft.EntityFrameworkCore;
using Webshop.Api.Models;

namespace Webshop.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {

        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<User> Users {  get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders {  get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
    }
}

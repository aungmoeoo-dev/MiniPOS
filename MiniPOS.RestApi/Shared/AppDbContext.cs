using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MiniPOS.RestApi.Features.Category;
using MiniPOS.RestApi.Features.Product;
using MiniPOS.RestApi.Features.Sale;
using MiniPOS.RestApi.Features.SaleDetail;

namespace MiniPOS.RestApi.Shared;

public class AppDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(AppSettings.ConnectionString);
    }

    public DbSet<ProductModel> Products { get; set; }
    public DbSet<CategoryModel> Categories { get; set; }
    public DbSet<SaleModel> Sales { get; set; }
    public DbSet<SaleDetailModel> SaleDetails { get; set; }
}

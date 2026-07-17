using Microsoft.EntityFrameworkCore;
using DMAS_EXAM.Models;

namespace DMAS_EXAM.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Asset> Assets { get; set; }
    public DbSet<PlayerAsset> PlayerAssets { get; set; }
}
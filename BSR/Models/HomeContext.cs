using Microsoft.EntityFrameworkCore;

namespace BSR.Models;

public class HomeContext: DbContext
{
    public HomeContext(DbContextOptions<HomeContext> options): base(options) { }

    public DbSet<Home> Homes { get; set; }
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace PessoasInfo.Data;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Telefone> Telefones { get; set; }
    public DbSet<Detalhe> Detalhes { get; set; }

    // Fluet API setup for Identity
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
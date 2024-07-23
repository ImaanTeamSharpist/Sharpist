using Microsoft.EntityFrameworkCore;
using Sharpist.Server.Domain.Entities;

namespace Sharpist.Server.Data.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }
    public DbSet<Vacancy> Vacancies { get; set; }
    public DbSet<Resume> Resumes { get; set; }
    public DbSet<HR> HRs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Sharpist.Server.Data.Contexts;
using Sharpist.Server.Data.IRepositories;
using Sharpist.Server.Domain.Entities.Commons;

namespace Sharpist.Server.Data.Repositories;

public class Repository<TEnitity> : IRepository<TEnitity> where TEnitity : Auditable
{
    AppDbContext dbContext;
    DbSet<TEnitity> dbSet;
    public Repository(AppDbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = dbContext.Set<TEnitity>();
    }
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await dbSet.FirstOrDefaultAsync(e => e.Id == id);
        dbSet.Remove(entity);
        return true;

    }
    public async Task<TEnitity> InsertAsync(TEnitity enitity)
        => (await dbSet.AddAsync(enitity)).Entity;

    public async Task<bool> SaveAsync()
        => await dbContext.SaveChangesAsync() > 0;

    public IQueryable<TEnitity> SelectAll(Expression<Func<TEnitity, bool>> expression = null, string[] includes = null)
    {
        var query = expression is null ? dbSet : dbSet.Where(expression);
        if (includes is not null)
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query;
    }

    public async Task<TEnitity> SelectAsync(Expression<Func<TEnitity, bool>> expression, string[] includes = null)
        => await SelectAll(expression, includes).FirstOrDefaultAsync();

    public TEnitity Update(TEnitity enitity)
        => dbSet.Update(enitity).Entity;

}

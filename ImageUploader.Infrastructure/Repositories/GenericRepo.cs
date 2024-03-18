using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImageUploader.Infrastructure.Repositories
{
    //Repo generic qui peut etre hérité
    //Ca permet de facilement communiquer avec la BD
    //Method sont virtuel pour pouvoir override ca.
    public abstract class GenericRepo<T>: IRepository<T> where T : class
    {
        private readonly DbContext context;
        private readonly DbSet<T> entities;
        private readonly IConfiguration config;

        public GenericRepo(DbContext context, IConfiguration config)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            entities = context.Set<T>();
            this.config = config;
        }

        public async virtual Task<T> GetById(int id)
        {
            return await entities.FindAsync(id);
        }

        public virtual async Task<T> Add(T entity)
        {
            await entities.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> Update(T entity)
        {
            entities.Update(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task Delete(int id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                entities.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }
    }

    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<T> Add(T entity);
        Task<T> Update(T entity);
        Task Delete(int id);
    }
}

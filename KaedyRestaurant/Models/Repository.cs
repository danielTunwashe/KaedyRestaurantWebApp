
using KaedyRestaurant.Data;
using Microsoft.EntityFrameworkCore;

namespace KaedyRestaurant.Models
{
    public class Repository<T> : IRepository<T> where T : class
    {
        // we need our context
        protected ApplicationDbContext _context { get; set; }

        //we also need what table we will be working with which would be passed in as type T
        private DbSet<T> _dbSet {  get; set; }

        // build a constructor to initialize the two things
        public Repository (ApplicationDbContext context)
        {
            _context = context; // Used to make the connection to the db
            _dbSet =  context.Set<T>(); // Specify the table we are working with
        }
        public async Task AddAsync(T entity)
        {
            //define our table type
            await _dbSet.AddAsync(entity); // adding our entity to our table
            await _context.SaveChangesAsync();  // saving the changes
        }

        public async Task DeleteAsync(int id)
        {
            //recieves the id, look the ID up out of the database table, remove it then save changes
            T entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet; // Define what type we are querying
            if (options.HasWhere)
            {
                query = query.Where(options.where);
            }
            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }
            foreach(string include in options.GetIncludes())
            {
                query = query.Include(include);
            }
            
            var key = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties.FirstOrDefault();
            string primaryKeyName = key?.Name;
            return await query.FirstOrDefaultAsync(e => EF.Property<int>(e, primaryKeyName) == id);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAllByIdAsync<TKey>(TKey id, string propertyName, QueryOptions<T> options)
        {
            IQueryable<T> query = _dbSet;

            if (options.HasWhere)
            {
                query = query.Where(options.where);
            }


            if (options.HasOrderBy)
            {
                query = query.OrderBy(options.OrderBy);
            }

            foreach (string include in options.GetIncludes())
            {
                query = query.Include(include);
            }
            // Filter by the specified property name and id
            query = query.Where(e => EF.Property<TKey>(e, propertyName).Equals(id));

            return await query.ToListAsync();

        }
    }

}


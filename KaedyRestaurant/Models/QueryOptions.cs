using System.Linq.Expressions;

namespace KaedyRestaurant.Models
{
    public class QueryOptions<T> where T : class
    {
        // a class that accepts the links queries like the where clause, the orderby clause
        // helps us to parse link queries
        public Expression<Func<T, Object>> OrderBy { get; set; } = null;
        public Expression<Func<T, bool>> where { get; set; } = null;

        public string[] includes = Array.Empty<string>();

        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }

        public string[] GetIncludes() => includes;

        public bool HasWhere => where != null;

        public bool HasOrderBy => OrderBy != null;
    }
}
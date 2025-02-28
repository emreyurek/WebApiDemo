using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;

namespace Entities.Helpers
{
    public class SortHelper<T> : ISortHelper<T>
    {
        public IQueryable<T> ApplySort(IQueryable<T> entities, string orderByQueryString)
        {
            if (!entities.Any() || string.IsNullOrWhiteSpace(orderByQueryString))
                return entities;

            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyInfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var orderQueryBuilder = new StringBuilder();

            foreach (var param in orderParams)
            {
                if (string.IsNullOrWhiteSpace(param))
                    continue;

                var propertyFromQueryName = param.Split()[0];
                var objectProperty = propertyInfo.FirstOrDefault(pi => pi.Name.Equals(propertyFromQueryName, StringComparison.InvariantCultureIgnoreCase));

                if (objectProperty is null)
                    continue;

                var sortingOrder = param.EndsWith(" desc") ? "desc" : "asc";

                orderQueryBuilder.Append($"{objectProperty.Name.ToString()} {sortingOrder}, ");
            }

            var orderQuery = orderQueryBuilder.ToString().TrimEnd(',', ' ');
            if (string.IsNullOrWhiteSpace(orderQuery))
                return entities;

            return entities.OrderBy(orderQuery);
        }
    }
}

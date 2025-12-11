using Microsoft.EntityFrameworkCore;

public static class QueryableExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> query, string sortBy, string sortDirection)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
            return query;

        var isDescending = sortDirection?.ToLower() == "desc";

        var property = typeof(T).GetProperty(sortBy,
            System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

        if (property == null)
            return query;

        return isDescending
            ? query.OrderByDescending(e => EF.Property<object>(e, property.Name))
            : query.OrderBy(e => EF.Property<object>(e, property.Name));
    }

    public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
    }
}

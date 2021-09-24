using System.Linq;
using ApiDPSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiDPSystem.Extensions
{
    public static class FilterExtension
    {
        public static IQueryable<Entities.Car> ApplyFilter(this IQueryable<Entities.Car> query, Filter filter)
        {
            if (filter == null)
            {
                return query;
            }

            if (!string.IsNullOrEmpty(filter.DealerName))
            {
                query = query.Include(c => c.Dealer);
                query = query.Where(p => p.Dealer.Name == filter.DealerName);
            }

            query = filter.Category switch
                   {
                       Category.Disabled => query,
                       Category.Actual => query.Where(p => p.IsActual),
                       Category.Sold => query.Where(p => p.IsSold),
                       _ => query
                   };

            return query;
        }
    }
}
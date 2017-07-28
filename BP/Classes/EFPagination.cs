using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BP.Classes
{
    public class EFPagination
    {
        public int TotalItems { get; private set; } // Equal to PageSize * MaxPage
        public int PageSize { get; private set; } // Number of items per page
        public int MinPage { get; private set; } // Page index starting from MinPage to MaxPage
        public int MaxPage { get; private set; } //Total pages

        public EFPagination(int totalItems, int itemsPerPage)
        {
            if (itemsPerPage < MinPage)
            {
                throw new ArgumentOutOfRangeException(null, "*** Number of items per page must > 0! ***");
            }

            TotalItems = totalItems;
            PageSize = itemsPerPage;
            MaxPage = CalculateTotalPages(totalItems, itemsPerPage);
        }

        private int CalculateTotalPages(int totalItems, int itemsPerPage)
        {
            int totalPages = totalItems / itemsPerPage;

            if (totalItems % itemsPerPage != 0)
            {
                totalPages++; // Last page with only 1 item left
            }

            return totalPages;
        }
    }

    public static class EFPaginationExtensions
    {
        public static IQueryable<T> PagedIndex<T>(this IQueryable<T> query, EFPagination pagination, int pageIndex) //where T : Entity
        {
            if (pageIndex < pagination.MinPage || pageIndex > pagination.MaxPage)
            {
                throw new ArgumentOutOfRangeException(null, "*** Page index must >= {pagination.MinPage} and =< {pagination.MaxPage}! ***");
            }

            // Return IQueryable<T> to enable chained-methods calls
            return query
                //.OrderBy(T => T.EntityId) [NOT this extension responsibility]
                .Skip(GetSkip(pageIndex, pagination.PageSize))
                .Take(pagination.PageSize);
        }

        private static int GetSkip(int pageIndex, int take)
        {
            return (pageIndex - 1) * take;
        }
    }
}
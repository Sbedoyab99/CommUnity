﻿using CommUnity.Shared.DTOs;

namespace CommUnity.BackEnd.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordsNumber)
                .Take(pagination.RecordsNumber);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationPqrsDTO paginationPqrs)
        {
            return queryable
                .Skip((paginationPqrs.Page - 1) * paginationPqrs.RecordsNumber)
                .Take(paginationPqrs.RecordsNumber);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationMailDTO paginationMailDTO)
        {
            return queryable
                .Skip((paginationMailDTO.Page - 1) * paginationMailDTO.RecordsNumber)
                .Take(paginationMailDTO.RecordsNumber);
        }

        public static IQueryable<T> Paginate<T>(this IQueryable<T> queryable, PaginationVisitorDTO paginationVisitorDTO)
        {
            return queryable
                .Skip((paginationVisitorDTO.Page - 1) * paginationVisitorDTO.RecordsNumber)
                .Take(paginationVisitorDTO.RecordsNumber);
        }

    }
}

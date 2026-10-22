/*using IW5_project.Api.DAL.Common.Entities;
using IW5_project.API.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IW5_project.IW5_project.Api.DAL.Common.Queries
{
    public class OrderItems
    {
        //order items by ascending order by name
        public static IQueryable<ItemEntity> OrderByNameAscending(this IQueryable<ItemEntity> query)
        {
            return query.OrderBy(item => item.Name);
        }

        //order items by descending order by name
        public static IQueryable<ItemEntity> OrderByNameDescending(this IQueryable<ItemEntity> query)
        {
            return query.OrderByDescending(item => item.Name);
        }

        //order items by descending order by user
        /*public static IQueryable<ItemEntity> OrderByUserDescending(this IQueryable<ItemEntity> query)
        {
        return query.OrderByDescending(item => item.UserId);
        }

        //order items by ascending order by user
        public static IQueryable<ItemEntity> OrderByUserAscending(this IQueryable<ItemEntity> query)
        {
        return query.OrderBy(item => item.UserId);
        }*/

        //order items by descending order by place
        /*public static IQueryable<ItemEntity> OrderByUserDescending(this IQueryable<ItemEntity> query)
        {
        return query.OrderByDescending(item => item.Id);
        }

        //order items by ascending order by user
        public static IQueryable<ItemEntity> OrderByUserAscending(this IQueryable<ItemEntity> query)
        {
        return query.OrderBy(item => item.Id);
        }*/
/*
        //order by amount ascending
        public static IQueryable<ItemEntity> OrderByAmountAscending(this IQueryable<ItemEntity> query)
        {
            return query.OrderBy(item => item.Quantity);
        }

        //order by amount descending
        public static IQueryable<ItemEntity> OrderByAmountDescending(this IQueryable<ItemEntity> query)
        {
            return query.OrderByDescending(item => item.Quantity);
        }
    }
}*/
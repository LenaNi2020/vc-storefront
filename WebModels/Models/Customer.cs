﻿#region

using System;
using System.Collections.Generic;
using System.Linq;
using DotLiquid;
using System.Threading.Tasks;
using VirtoCommerce.Web.Models.Services;
using VirtoCommerce.Web.Convertors;

#endregion

namespace VirtoCommerce.Web.Models
{
    public class Customer : Drop, ILoadSlice
    {
        #region Constructors and Destructors
        public Customer()
        {
            this.Addresses = new List<CustomerAddress>();
        }
        #endregion

        #region Public Properties
        public bool AcceptsMarketing { get; set; }

        public List<CustomerAddress> Addresses { get; set; }

        public int AddressesCount
        {
            get
            {
                return this.Addresses == null ? 0 : this.Addresses.Count;
            }
        }

        public CustomerAddress DefaultAddress { get; set; }

        public CustomerAddress NewAddress
        {
            get
            {
                return new CustomerAddress() {Id = Guid.NewGuid().ToString()};
            }
        }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public bool HasAccount { get; set; }

        public string Id { get; set; }

        public string LastName { get; set; }

        public string LastOrder { get; set; }

        public string Name { get; set; }

        public ItemCollection<CustomerOrder> Orders { get; set; }

        public int OrdersCount { get; set; }

        public CustomerOrder RecentOrder { get; set; }

        public ICollection<Tag> Tags { get; set; }

        public decimal TotalSpent { get; set; }
        #endregion

        public void LoadSlice(int from, int? to)
        {
            var pageSize = to == null ? 5 : to - from;

            var customerService = new CustomerService();

            var orderSearchResult =
                Task.Run(() => customerService.GetOrdersAsync(
                    SiteContext.Current.StoreId,
                    Id,
                    null,
                    from,
                    pageSize.Value)).Result;

            var test = SiteContext.Current;

            var orders = orderSearchResult.CustomerOrders.Select(o => o.AsWebModel());
            var ordersCollection = new ItemCollection<CustomerOrder>(orders)
            {
                TotalCount = orderSearchResult.TotalCount
            };

            Orders = ordersCollection;
        }
    }
}
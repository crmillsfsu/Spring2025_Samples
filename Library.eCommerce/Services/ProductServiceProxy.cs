using Library.eCommerce.DTO;
using Library.eCommerce.Models;
using Library.eCommerce.Util;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private ProductServiceProxy()
        {           
            
            Products = new List<Item?>
            {
                new Item{ Product = new ProductDTO{Id = 1, Name ="F", Price = 5.00m}, Id = 1, Quantity = 1, Price = 5.00m },
                new Item{ Product = new ProductDTO{Id = 2, Name ="Z", Price = 1.00m}, Id = 2 , Quantity = 2, Price = 1.00m },
                new Item{ Product = new ProductDTO{Id = 3, Name ="A", Price = 9.99m}, Id=3 , Quantity = 3 , Price = 9.99m}
            };
        }

        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();
        public static ProductServiceProxy Current
        {
            get
            {
                lock(instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                }

                return instance;
            }
        }

        public List<Item?> Products { get; private set; }

        public async Task<IEnumerable<Item?>> Search(string? query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Products;
            }

            var results = Products
                .Where(item => item != null &&
                               item.Product != null &&
                               !string.IsNullOrWhiteSpace(item.Product.Name) &&
                               item.Product.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return results;
        }

        public Item AddOrUpdate(Item item)
        {
            
            if(item == null)
            {
                return item;
            }
            if (item.Id == 0)
            {
                int newId = Products.Any() ? Products.Max(p => p?.Id ?? 0) + 1 : 1;
                item.Id = newId;
                if (item.Product != null)
                {
                    item.Product.Id = newId;
                    item.Product.Price = item.Price;
                }
                Products.Add(item);
            }
            else
            {
                var existingItem = Products.FirstOrDefault(p => p.Id == item.Id);
                var index = Products.IndexOf(existingItem);
                if (item.Product != null)
                {
                    item.Product.Price = item.Price;
                }
                Products.RemoveAt(index);
                Products.Insert(index,new Item(item));
            }


            return item;
        }

        public Item? Delete(int id)
        {
            if(id == 0)
            {
                return null;
            }


            Item? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return product;
        }

        public Item? GetById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

    }

    
}

using Library.eCommerce.DTO;
using Library.eCommerce.Models;
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
            new Item{ Product = new ProductDTO{ Id = 1, Name = "Bluetooth Mouse", Price = 24.99m }, Id = 1, Quantity = 10 },
            new Item{ Product = new ProductDTO{ Id = 2, Name = "Mechanical Keyboard", Price = 79.99m }, Id = 2, Quantity = 5 },
            new Item{ Product = new ProductDTO{ Id = 3, Name = "Laptop Stand", Price = 39.99m }, Id = 3, Quantity = 7 },
            new Item{ Product = new ProductDTO{ Id = 4, Name = "USB-C Hub", Price = 29.99m }, Id = 4, Quantity = 8 },
            new Item{ Product = new ProductDTO{ Id = 5, Name = "Noise Cancelling Headphones", Price = 129.99m }, Id = 5, Quantity = 4 }
            };
        }


        private int LastKey
        {
            get
            {
                if(!Products.Any())
                {
                    return 0;
                }

                return Products.Select(p => p?.Id ?? 0).Max();
            }
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


        public Item AddOrUpdate(Item item)
        {
            //CALL THE WEB SERVICE
            var response = new WebRequestHandler().Post("/Inventory", item).Result;
            var newItem = JsonConvert.DeserializeObject<Item>(response);
            if(newItem == null)
            {
                return item;
            }
            if(item.Id == 0)
            {
                Products.Add(newItem);
            } else
            {
                var existingItem = Products.FirstOrDefault(p => p.Id == item.Id);
                var index = Products.IndexOf(existingItem);
                Products.RemoveAt(index);
                Products.Insert(index,new Item(newItem));
            }


            return item;
        }

        public Item? Delete(int id)
        {
            if(id == 0)
            {
                return null;
            }

            var result = new WebRequestHandler().Delete($"/Inventory/{id}").Result;

            Item? product = Products.FirstOrDefault(p => p.Id == id);
            Products.Remove(product);

            return JsonConvert.DeserializeObject<Item>(result);
        }

        public Item? GetById(int id)
        {
            return Products.FirstOrDefault(p => p.Id == id);
        }

    }

    
}

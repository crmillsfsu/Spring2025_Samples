using Library.eCommerce.DTO;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Library.eCommerce.Models
{
    public class Item
    {
        public int Id { get; set; }
        public ProductDTO Product { get; set; }
        public int? Quantity { get; set; }

        public int Price { get; set; } 



        public override string ToString()
        {
            return $"{Product} Quantity:{Quantity}";
        }

        public string Display { 
            get
            {
                return $"{Product?.Display ?? string.Empty} x{Quantity} ${Price}";
            }
        }

                public int AddCertainAmount(int amount)
        {
            for(int i = 0; i < amount; i++)
            {
                if (ShoppingCartService.Current.AddOrUpdate(this) == null)
                {
                    return 0;
                }
            }
            return 1;
        }

        public Item()
        {
            Product = new ProductDTO();
            Quantity = 0;


        }

        public void DoAdd()
        {
            ShoppingCartService.Current.AddOrUpdate(this);
        }



        public Item(Item i)
        {
            Product = new ProductDTO(i.Product);
            Quantity = i.Quantity;
            Id = i.Id;
            Price = i.Price;

        }

        
    }
}

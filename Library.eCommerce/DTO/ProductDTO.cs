using Spring2025_Samples.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.eCommerce.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Display
        {
            get
            {
                return $"{Id}. {Name}";
            }
        }



        public ProductDTO()
        {
            Name = string.Empty;
        }

        public ProductDTO(Product p)
        {
            Name = p.Name;
            Id = p.Id;
        }

        public ProductDTO(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}

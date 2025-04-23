using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Spring2025_Samples.Models;

namespace Library.eCommerce.Services
{
    public class ShopListService
    {
        public List<ShoppingCart> Carts { get; private set; }
        public int currentCartId { get; set; } = 0;

        public static ShopListService Current 
        { 
            get
            {
                if (instance == null)
                {
                    instance = new ShopListService();
                }
                return instance;
            }
        } 
        private static ShopListService? instance;

        private ShopListService() { 
            Carts = new List<ShoppingCart>();
            Carts.Add(new ShoppingCart(0, ShoppingCartService.Current));
            //Adds the home shopping cart to the cart
        }

        public int GetNextId()
        {
            return Carts.Max(p => p.Id) + 1;
        }

        public void AddToShoppingCart()
        {
            ShoppingCartService newService = ShoppingCartService.AddNewShoppingCart();
            Carts.Add(new ShoppingCart(GetNextId(), newService));
        }

        public ShoppingCartService? ReturnCurrentList()
        {
            return Carts[currentCartId]?.ShopService;
        }

    }
}

using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
       private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
       private ShoppingCartService _cartSvc = ShoppingCartService.Current;
       public Item? SelectedItem { get; set; }
       public Item? SelectedCartItem { get; set; }

        public ObservableCollection<Item?> Inventory
        {
            get
            {
                return new ObservableCollection<Item?>(_invSvc.Products
                    .Where(i => i?.Quantity > 0)
                    );
            }
        }

        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                return new ObservableCollection<Item?>(_cartSvc.CartItems
                    .Where(i => i?.Quantity > 0)
                    );
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshUX()
        {
            NotifyPropertyChanged(nameof(Inventory));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem);

                if(updatedItem != null && shouldRefresh) {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }

            }
        }

        public void ReturnItem()
        {
            if (SelectedCartItem != null) {
                var shouldRefresh = SelectedCartItem.Quantity >= 1;
                
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }
        //Do stuff on list, then the next page will be a readout
        public void Checkout()  
        {
            /*    int totalCost = 0;
                foreach (var item in ShoppingCart)
                {
                    totalCost += item?.Price ?? 0;
                }
        //        _cartSvc.ClearList();

                if (totalCost > 0)
                {
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
                _cartSvc.CheckoutPrice = totalCost;
                NotifyPropertyChanged(nameof(_cartSvc.CheckoutPrice));
            */
        //    _cartSvc.ClearList();
            NotifyPropertyChanged(nameof(ShoppingCart));

        }

    }
}

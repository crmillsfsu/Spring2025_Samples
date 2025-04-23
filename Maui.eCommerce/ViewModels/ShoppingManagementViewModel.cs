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
using System.Windows.Input;



namespace Maui.eCommerce.ViewModels
{
    public class ShoppingManagementViewModel : INotifyPropertyChanged
    {
       private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
       private ShoppingCartService _cartSvc = ShoppingCartService.Current;

       private ShopListService _shopSvc = ShopListService.Current;

       public ItemViewModel? SelectedItem { get; set; }
       public ItemViewModel? SelectedCartItem { get; set; }
       public string mode = "Name";

        public ObservableCollection<ItemViewModel?> Inventory
        {
            get
            {
                var filteredList = _invSvc.Products
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m));
                if (mode == "Name")
                {
                    filteredList = filteredList?.OrderBy(I => I?.Model?.Product?.Name);
                }
                else
                {
                    filteredList = filteredList.OrderBy(I => I?.Model?.Price);
                }
                return new ObservableCollection<ItemViewModel?>(filteredList);
            }
        }


        public ObservableCollection<ItemViewModel?> ShoppingCart
        {
            get
            {
                var filteredList = _shopSvc.ReturnCurrentList().CartItems
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m));
                if (mode == "Name")
                {
                    filteredList = filteredList?.OrderBy(I => I?.Model?.Product?.Name);
                }
                else
                {
                    filteredList = filteredList.OrderBy(I => I?.Model?.Price);
                }
                return new ObservableCollection<ItemViewModel?>(filteredList);
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
                var shouldRefresh = SelectedItem.Model.Quantity >= 1;
                var updatedItem = _shopSvc.ReturnCurrentList().AddOrUpdate(SelectedItem.Model);

                if(updatedItem != null && shouldRefresh) {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }

            }
        }


        public void ReturnItem()
        {
            if (SelectedCartItem != null) {
                var shouldRefresh = SelectedCartItem.Model.Quantity >= 1;
                
                var updatedItem = _shopSvc.ReturnCurrentList().ReturnItem(SelectedCartItem.Model);

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

        public void changeFilterMode()
        {
            if (mode == "Name")
            {
                mode = "Price";
            }
            else
            {
                mode = "Name";
            }
        }

    }
}

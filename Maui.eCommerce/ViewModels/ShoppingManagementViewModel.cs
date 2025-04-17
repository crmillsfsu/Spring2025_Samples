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
       public ItemViewModel? SelectedItem { get; set; }
       public ItemViewModel? SelectedCartItem { get; set; }

        public ObservableCollection<ItemViewModel?> Inventory
        {
            get
            {
                return new ObservableCollection<ItemViewModel?>(_invSvc.Products
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m))
                    );
            }
        }

        public ObservableCollection<string> CartNames { get; set; } = new() { "Default", "Wishlist", "Gift Ideas" };

        private string _selectedCartName = "Default";
        public string SelectedCartName
        {
            get => _selectedCartName;
            set
            {
                if (_selectedCartName != value)
                {
                    _selectedCartName = value;
                    _cartSvc.SetActiveCart(value);
                    NotifyPropertyChanged(nameof(SelectedCartName));
                    RefreshUX();
                }
            }
        }




        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                return new ObservableCollection<ItemViewModel?>(_cartSvc.CartItems
                    .Where(i => i?.Quantity > 0).Select(m => new ItemViewModel(m))
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
                var shouldRefresh = SelectedItem.Model.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem.Model);

                if(updatedItem != null && shouldRefresh) {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }

            }
        }

        public void Checkout()
        {
            var cartItems = ShoppingCartService.Current.CartItems;
            if (cartItems.Count == 0)
            {
                Application.Current.MainPage.DisplayAlert("Checkout", "Your cart is empty.", "OK");
                return;
            }

            decimal subtotal = 0;
            StringBuilder receipt = new StringBuilder();
            receipt.AppendLine("Itemized Receipt");
            receipt.AppendLine("---------------------------");

            foreach (var item in cartItems)
            {
                string name = item.Product?.Name ?? "Unnamed";
                decimal price = item.Product?.Price ?? 0;
                int quantity = item.Quantity ?? 0;
                decimal lineTotal = price * quantity;

                receipt.AppendLine($"{name} x{quantity} @ ${price:F2} = ${lineTotal:F2}");
                subtotal += lineTotal;
            }

            double taxRate = Preferences.Get("SalesTaxRate", 7.0);
            decimal tax = subtotal * ((decimal)taxRate / 100);
            decimal total = subtotal + tax;

            receipt.AppendLine("---------------------------");
            receipt.AppendLine($"Subtotal: ${subtotal:F2}");
            receipt.AppendLine($"Tax ({taxRate:F2}%): ${tax:F2}");
            receipt.AppendLine($"Total: ${total:F2}");

            Application.Current.MainPage.DisplayAlert("Receipt", receipt.ToString(), "OK");

            ShoppingCartService.Current.ClearCart();
            RefreshUX();
        }


        public void PurchaseItem(Item item, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                _cartSvc.AddOrUpdate(item);
            }

            RefreshUX();
        }


        public void ReturnItem()
        {
            if (SelectedCartItem != null) {
                var shouldRefresh = SelectedCartItem.Model.Quantity >= 1;
                
                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem.Model);

                if (updatedItem != null && shouldRefresh)
                {
                    NotifyPropertyChanged(nameof(Inventory));
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }
    }
}

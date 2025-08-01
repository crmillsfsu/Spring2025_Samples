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
            NotifyPropertyChanged(nameof(CartSubtotal));
            NotifyPropertyChanged(nameof(CartTotal));
        }

        public void PurchaseItem()
        {
            if (SelectedItem != null)
            {
                var shouldRefresh = SelectedItem.Model.Quantity >= 1;
                var updatedItem = _cartSvc.AddOrUpdate(SelectedItem.Model);

                if (updatedItem != null && shouldRefresh)
                {
                    RefreshUX();
                }
            }
        }

        public void ReturnItem()
        {
            if (SelectedCartItem != null)
            {
                var shouldRefresh = SelectedCartItem.Model.Quantity >= 1;

                var updatedItem = _cartSvc.ReturnItem(SelectedCartItem.Model);

                if (updatedItem != null && shouldRefresh)
                {
                    RefreshUX();
                }
            }
        }

        public enum SortMode
        {
            Name,
            Price
        }

        private SortMode _inventorySortMode = SortMode.Name;
        public SortMode InventorySortMode
        {
            get => _inventorySortMode;
            set
            {
                if (_inventorySortMode != value)
                {
                    _inventorySortMode = value;
                    NotifyPropertyChanged(nameof(Inventory));
                }
            }
        }

        private SortMode _cartSortMode = SortMode.Name;
        public SortMode CartSortMode
        {
            get => _cartSortMode;
            set
            {
                if (_cartSortMode != value)
                {
                    _cartSortMode = value;
                    NotifyPropertyChanged(nameof(ShoppingCart));
                }
            }
        }

        public ObservableCollection<ItemViewModel?> Inventory
        {
            get
            {
                IEnumerable<ItemViewModel?> items = _invSvc.Products
                    .Where(p => p != null)
                    .Select(m => new ItemViewModel(m));

                switch (InventorySortMode)
                {
                    case SortMode.Price:
                        items = items.OrderBy(i => i.Price);
                        break;
                    default:
                        items = items.OrderBy(i => i.Model.Product.Name);
                        break;
                }

                return new ObservableCollection<ItemViewModel?>(items);
            }
        }

        public ObservableCollection<ItemViewModel?> ShoppingCart
        {
            get
            {
                IEnumerable<ItemViewModel?> items = _cartSvc.CartItems
                    .Where(i => i?.Quantity > 0)
                    .Select(m => new ItemViewModel(m));

                switch (CartSortMode)
                {
                    case SortMode.Price:
                        items = items.OrderBy(i => i.Price);
                        break;
                    default:
                        items = items.OrderBy(i => i.Model.Product.Name);
                        break;
                }

                return new ObservableCollection<ItemViewModel?>(items);
            }
        }

        public decimal CartSubtotal
        {
            get
            {
                return ShoppingCart
                    .Where(i => i != null)
                    .Sum(i => i!.Price);
            }
        }

        public decimal CartTotal
        {
            get
            {
                return ShoppingCart
                    .Where(i => i != null)
                    .Sum(i => (i!.Price) * (i.Model.Quantity ?? 0));
            }
        }
    }
}

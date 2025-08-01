using Library.eCommerce.Models;
using Library.eCommerce.Services;
using Spring2025_Samples.Models;
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
    public class InventoryManagementViewModel : INotifyPropertyChanged
    {
        public Item? SelectedProduct { get; set; }
        public string? Query { get; set; }
        private ProductServiceProxy _svc = ProductServiceProxy.Current;

        public event PropertyChangedEventHandler? PropertyChanged;

        public enum InventorySortMode
        {
            Name,
            Price
        }

        private InventorySortMode _sortMode = InventorySortMode.Name;
        public InventorySortMode SortMode
        {
            get => _sortMode;
            set
            {
                if (_sortMode != value)
                {
                    _sortMode = value;
                    NotifyPropertyChanged(nameof(Products));
                }
            }
        }

        public ObservableCollection<Item?> Products
        {
            get
            {
                IEnumerable<Item?> filteredList = _svc.Products
                    .Where(p => (p?.Quantity ?? 0) > 0) 
                    .Where(p => p?.Product?.Name?.ToLower()
                    .Contains(Query?.ToLower() ?? string.Empty) ?? false);

                switch (SortMode)
                {
                    case InventorySortMode.Price:
                        filteredList = filteredList.OrderBy(p => p?.Price);
                        break;
                    default:
                        filteredList = filteredList.OrderBy(p => p?.Product?.Name);
                        break;
                }

                return new ObservableCollection<Item?>(filteredList);
            }
        }




        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void RefreshProductList()
        {
            NotifyPropertyChanged(nameof(Products));
        }

        public async Task<bool> Search()
        {
            await _svc.Search(Query);
            NotifyPropertyChanged(nameof(Products));
            return true;
        }
        public Item? Delete()
        {
            var item = _svc.Delete(SelectedProduct?.Id ?? 0);
            NotifyPropertyChanged("Products");
            return item;
        }
    }
}

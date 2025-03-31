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

        public ObservableCollection<Item?> Products
        {
            get
            {
                var filteredList = _svc.Products.Where(p => p?.Product?.Name?.ToLower().Contains(Query?.ToLower() ?? string.Empty) ?? false);
                return new ObservableCollection<Item?>(filteredList);
            }
        }

        public Item? Delete()
        {
            var item = _svc.Delete(SelectedProduct?.Id ?? 0);
            NotifyPropertyChanged("Products");
            return item;
        }
    }
}

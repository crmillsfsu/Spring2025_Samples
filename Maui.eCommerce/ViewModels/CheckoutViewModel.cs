// Maui.eCommerce/ViewModels/CheckoutViewModel.cs

using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Maui.eCommerce.ViewModels
{
    public class CheckoutViewModel : INotifyPropertyChanged
    {
        private ShoppingCartService _cartSvc = ShoppingCartService.Current;
        private readonly double _taxRate;

        public class ReceiptItemViewModel : INotifyPropertyChanged
        {
            public Item Model { get; }
            public string Name => Model?.Product?.Name ?? "Unknown";
            public decimal Price => Model?.Price ?? 0;
            public int Quantity => Model?.Quantity ?? 0;
            public decimal LineTotal => Price * Quantity;

            public ReceiptItemViewModel(Item item)
            {
                Model = item;
            }

            public event PropertyChangedEventHandler? PropertyChanged;
            protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
                => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ReceiptItemViewModel> CartItems { get; private set; }

        public decimal Subtotal
        {
            get => CartItems.Sum(item => item.LineTotal);
        }

        public decimal TaxAmount
        {
            get => Subtotal * (decimal)(_taxRate / 100.0);
        }

        public decimal Total
        {
            get => Subtotal + TaxAmount;
        }

        public double TaxRateValue => _taxRate;

        public string OrderNumber { get; private set; }

        public CheckoutViewModel()
        {
            
            _taxRate = Preferences.Get("TaxRate", 7.0);

            
            CartItems = new ObservableCollection<ReceiptItemViewModel>(
                _cartSvc.CartItems
                    .Where(i => i?.Quantity > 0)
                    .Select(m => new ReceiptItemViewModel(m))
            );

        }

        public void CompleteOrder()
        {
            foreach (var item in _cartSvc.CartItems.ToList())
            {
                while (item.Quantity > 0)
                {
                    _cartSvc.ReturnItem(item);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

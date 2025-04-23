using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Models;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class TotalCostViewModel : INotifyPropertyChanged
    { 
        private ShopListService _shopsv = ShopListService.Current;
    
        private ShoppingCartService __svc;

        public event PropertyChangedEventHandler? PropertyChanged;

        public double CheckoutCost
        {
            get {
                __svc = _shopsv.ReturnCurrentList();
                var rounded = Math.Round(__svc.CheckoutPrice + ((__svc.CheckoutPrice * __svc.taxRate) /100), 2);
                return rounded;
                }
            set 
            {
                __svc.CheckoutPrice = value;

            }
        }

        public ObservableCollection<Item?> ShoppingCart
        {
            get
            {
                __svc = _shopsv.ReturnCurrentList();
                int totalCost = 0;
                foreach (var item in __svc.CartItems)
                {
                    totalCost += item?.Price ?? 0;
                }
                CheckoutCost = totalCost;
                NotifyPropertyChanged(nameof(CheckoutCost));

                var toRet =  new ObservableCollection<Item?>(__svc.CartItems
                    .Where(i => i?.Quantity > 0)
                    );
    //            __svc.CartItems.Clear();

                

                return toRet;
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

        public void RefreshUI()
        {
            NotifyPropertyChanged(nameof(CheckoutCost));
            NotifyPropertyChanged(nameof(ShoppingCart));
        }
        public void ClearOnLeave()
        {
            __svc.CartItems.Clear();
            CheckoutCost = 0;
        }
    }
}

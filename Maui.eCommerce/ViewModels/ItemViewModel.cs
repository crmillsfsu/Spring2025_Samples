using Library.eCommerce.Models;
using Library.eCommerce.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maui.eCommerce.ViewModels
{
    public class ItemViewModel
    {
        public Item Model { get; set; }

        public string? boxValue { get; set; } = "1";

        public ICommand? AddCommand { get; set; }

        public ICommand? AddCustomAmountClicked { get; set; }

        private ProductServiceProxy _invSvc = ProductServiceProxy.Current;
        private ShoppingCartService _cartSvc = ShoppingCartService.Current;

        private void DoAdd()
        {
            ShoppingCartService.Current.AddOrUpdate(Model);
        }

        private void AddCustomInCart()
        {
            if (boxValue == null ) { return ; }
            int value = int.Parse(boxValue);

            if (value < 1) { return; }

            for (int i = 0; i < value; i++)
            {
                _cartSvc.AddOrUpdate(Model);
            }
        }

        void SetupCommands()
        {
            AddCommand = new Command(DoAdd);
            AddCustomAmountClicked = new Command(AddCustomInCart);
        }

        public ItemViewModel()
        {
            Model = new Item();

            SetupCommands();
        }

        public ItemViewModel(Item model)
        {
            Model = model;
            SetupCommands();
        }
    }
}
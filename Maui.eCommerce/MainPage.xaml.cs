using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private void InventoryClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//InventoryManagement");
        }

        private void ShopClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//ShoppingManagement");
        }
    }

}

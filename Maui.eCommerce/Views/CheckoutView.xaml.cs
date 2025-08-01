// Maui.eCommerce/Views/CheckoutView.xaml.cs
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views
{
    public partial class CheckoutView : ContentPage
    {
        public CheckoutView()
        {
            InitializeComponent();
            BindingContext = new CheckoutViewModel();
        }

        private async void CompleteOrderClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Complete Order", "Do you want to complete this order?", "Yes", "No");

            if (confirm)
            {
                (BindingContext as CheckoutViewModel)?.CompleteOrder();
                await DisplayAlert("Success", "Order completed successfully!", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}

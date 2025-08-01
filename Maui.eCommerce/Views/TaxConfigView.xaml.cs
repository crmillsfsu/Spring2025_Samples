using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views
{
    public partial class TaxConfigView : ContentPage
    {
        public TaxConfigView()
        {
            InitializeComponent();
            BindingContext = new TaxConfigViewModel();
        }

        private void SaveClicked(object sender, EventArgs e)
        {
            (BindingContext as TaxConfigViewModel)?.SaveTaxRate();
        }

        private async void GoBackClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}

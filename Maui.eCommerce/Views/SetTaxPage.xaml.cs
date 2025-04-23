using Maui.eCommerce.ViewModels;


namespace Maui.eCommerce.Views;

public partial class SetTaxPage : ContentPage
{
	public SetTaxPage()
	{
		InitializeComponent();
		BindingContext = new TaxPageViewModel();
	}

	public void SetTaxClicked(object sender, EventArgs e)
	{
        (BindingContext as TaxPageViewModel).set_tax_input();
        Shell.Current.GoToAsync($"//MainPage");
    }
}
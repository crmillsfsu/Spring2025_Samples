using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShoppingManagementView : ContentPage
{
	public ShoppingManagementView()
	{
		InitializeComponent();
		BindingContext = new ShoppingManagementViewModel();
	}

    private void RemoveFromCartClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel).ReturnItem();
    }
    private void AddToCartClicked(object sender, EventArgs e)
    {
		(BindingContext as ShoppingManagementViewModel).PurchaseItem();
    }

    private void InlineAddClicked(object sender, EventArgs e)
    {
        (BindingContext as ShoppingManagementViewModel).RefreshUX();
    }

    public void GoToCheckoutClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync($"//CheckoutPage");
        (BindingContext as ShoppingManagementViewModel).Checkout();
    }
}
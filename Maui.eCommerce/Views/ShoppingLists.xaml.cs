using Library.eCommerce.Models;
using Maui.eCommerce.ViewModels;

namespace Maui.eCommerce.Views;

public partial class ShoppingLists : ContentPage
{
    public ShoppingLists()
	{
		InitializeComponent();
		BindingContext = new CartListViewModel();
	}

	public void AddNewShoppingCart(object sender, EventArgs e)
    {
        (BindingContext as CartListViewModel).AddNewCart();
    }

	public void GoBackToShoppingCart(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//ShoppingManagement");
    }

	public void switchShoppingCart(object sender, EventArgs e)
	{
		(BindingContext as CartListViewModel).ChangeCurrentShoppingCart();
    }
}
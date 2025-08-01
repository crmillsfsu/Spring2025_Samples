using Maui.eCommerce.ViewModels;

using static Maui.eCommerce.ViewModels.ShoppingManagementViewModel;

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
    private async void GoBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private void SortInventoryByNameClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ShoppingManagementViewModel;
        if (vm != null)
            vm.InventorySortMode = SortMode.Name;
    }

    private void SortInventoryByPriceClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ShoppingManagementViewModel;
        if (vm != null)
            vm.InventorySortMode = SortMode.Price;
    }

    private void SortCartByNameClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ShoppingManagementViewModel;
        if (vm != null)
            vm.CartSortMode = SortMode.Name;
    }

    private void SortCartByPriceClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ShoppingManagementViewModel;
        if (vm != null)
            vm.CartSortMode = SortMode.Price;
    }

    private async void CheckoutClicked(object sender, EventArgs e)
    {
        var vm = BindingContext as ShoppingManagementViewModel;

        if (vm?.ShoppingCart == null || vm.ShoppingCart.Count == 0)
        {
            await DisplayAlert("Empty Cart", "Your shopping cart is empty.", "OK");
            return;
        }

        await Shell.Current.GoToAsync(nameof(CheckoutView));
    }


}
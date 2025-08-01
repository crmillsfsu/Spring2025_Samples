using Maui.eCommerce.Views;

namespace Maui.eCommerce
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(CheckoutView), typeof(CheckoutView));
            InitializeComponent();
        }
    }
}

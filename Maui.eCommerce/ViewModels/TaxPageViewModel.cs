using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.Services;

namespace Maui.eCommerce.ViewModels
{
    public class TaxPageViewModel
    {
        public string ? tax_input { get; set; }
        public ShoppingCartService svc = ShoppingCartService.Current;

        public void set_tax_input()
        {
            if (tax_input == null) { return; }
            svc.taxRate = double.Parse(tax_input);
        }
    
    }


}

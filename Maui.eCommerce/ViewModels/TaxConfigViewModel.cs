using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Maui.eCommerce.ViewModels
{
    public class TaxConfigViewModel : INotifyPropertyChanged
    {
        private double _taxRate;
        private string _statusMessage;
        private const double DEFAULT_TAX_RATE = 7;

        public double TaxRate
        {
            get => _taxRate;
            set
            {
                if (_taxRate != value)
                {
                    _taxRate = value;
                    OnPropertyChanged();
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public TaxConfigViewModel()
        {
            TaxRate = Preferences.Get("TaxRate", DEFAULT_TAX_RATE);
        }

        public void SaveTaxRate()
        {
            Preferences.Set("TaxRate", TaxRate);
            StatusMessage = $"Tax rate saved: {TaxRate}%";
        }

        public void ResetToDefault()
        {
            TaxRate = DEFAULT_TAX_RATE;
            SaveTaxRate();
            StatusMessage = $"Tax rate reset to default: {TaxRate}%";
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

using System;
using DrawIOForms.Models;
using Xamarin.Forms;

namespace DrawIOForms.Views
{
    public partial class CreditCardEntryPage : ContentPage
    {
        public CreditCardEntryPage()
        {
            InitializeComponent();
        }

        public delegate void ScanSucceededEventHandler(object sender, CreditCard_PCL ccPCL);

        public event ScanSucceededEventHandler ScanSucceeded;
        public event EventHandler ScanCancelled;

        public CardIOConfig cardIOConfig;


        public void OnScanSucceeded(CreditCard_PCL ccPCL)
        {
            if (ScanSucceeded != null)
            {
                ScanSucceeded(this, ccPCL);
            }
        }

        public void OnScanCancelled()
        {
            if (ScanCancelled != null)
            {
                ScanCancelled(this, EventArgs.Empty);
            }
        }
    }
}



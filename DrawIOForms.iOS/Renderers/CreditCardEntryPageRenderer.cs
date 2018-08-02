using System;
using Card.IO;
using DrawIOForms.Models;
using DrawIOForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(DrawIOForms.Views.CreditCardEntryPage), typeof(DrawIOForms.iOS.Renderers.CreditCardEntryPageRenderer))]
namespace DrawIOForms.iOS.Renderers
{

    public class CreditCardEntryPageRenderer : PageRenderer, ICardIOPaymentViewControllerDelegate
    {

        private bool bViewAlreadyDisappeared = false;
        private CreditCardEntryPage ccPage;

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            try
            {
                ccPage = e.NewElement as CreditCardEntryPage;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"\t\t\tERROR: {ex.Message}");
            }
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // I don't know why ViewDidAppear keeps firing again, but we're able to shut it down 
            // by checking bViewAlreadyDisappeared.
            if (bViewAlreadyDisappeared) return;

            //var paymentDelegate = new CardIOPaymentViewControllerDg(ccPage);

            // Create and Show the View Controller
            //var paymentViewController = new CardIOPaymentViewController(paymentDelegate);
            CardIOPaymentViewController paymentViewController = new CardIOPaymentViewController(this);
            paymentViewController.CollectExpiry = ccPage.cardIOConfig.RequireExpiry;
            paymentViewController.CollectCVV = ccPage.cardIOConfig.RequireCvv;
            paymentViewController.CollectPostalCode = ccPage.cardIOConfig.RequirePostalCode;
            paymentViewController.HideCardIOLogo = ccPage.cardIOConfig.HideCardIOLogo;
            paymentViewController.CollectCardholderName = ccPage.cardIOConfig.CollectCardholderName;

            if (!string.IsNullOrEmpty(ccPage.cardIOConfig.Localization)) paymentViewController.LanguageOrLocale = ccPage.cardIOConfig.Localization;
            if (!string.IsNullOrEmpty(ccPage.cardIOConfig.ScanInstructions)) paymentViewController.ScanInstructions = ccPage.cardIOConfig.ScanInstructions;



            // Not sure if this needs to be diabled, but it doesn't seem like something I want to do.
            paymentViewController.AllowFreelyRotatingCardGuide = false;

            // Display the card.io interface
            PresentViewController(paymentViewController, true, null);

        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            bViewAlreadyDisappeared = true;
        }

        //private CreditCardEntryPage ccPage;
        private CreditCard_PCL ccPCL = new CreditCard_PCL();

        public void UserDidCancelPaymentViewController(CardIOPaymentViewController paymentViewController)
        {
            //App.Current
            //this.DismissViewController(true, null);
            paymentViewController.DismissViewController(true, null);
        }
        public void UserDidProvideCreditCardInfo(CreditCardInfo card, CardIOPaymentViewController paymentViewController)
        {
            //this.DismissViewController(true, null);
            paymentViewController.DismissViewController(true, null);

            if (card == null)
            {
                Console.WriteLine("Scanning Canceled!");

                ccPage.OnScanCancelled();
                //Xamarin.Forms.MessagingCenter.Send<CreditCard_PCL>(ccPCL, "CreditCardScanCancelled");

            }
            else
            {
                // Feel free to extend the CreditCard_PCL object to include more than what's here.
                ccPCL.cardNumber = card.CardNumber;
                ccPCL.ccv = card.Cvv;
                ccPCL.expr = card.ExpiryMonth.ToString() + card.ExpiryYear.ToString();
                ccPCL.redactedCardNumber = card.RedactedCardNumber;
                ccPCL.cardholderName = card.CardholderName;

                ccPage.OnScanSucceeded(ccPCL);
                //Xamarin.Forms.MessagingCenter.Send<CreditCard_PCL>(ccPCL, "CreditCardScanSuccess");

            }
        }


    }

}


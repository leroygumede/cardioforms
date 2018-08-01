using Android.App;
using Android.Content;
using Card.IO;
using DrawIOForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;

[assembly: ExportRenderer(typeof(DrawIOForms.Droid.Renderers.CreditCardEntryPageRenderer), typeof(DrawIOForms.Droid.Renderers.CreditCardEntryPageRenderer))]
namespace DrawIOForms.Droid.Renderers
{


    public class CreditCardEntryPageRenderer : PageRenderer
    {
        private CreditCardEntryPage ccPage;

        public CreditCardEntryPageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            Console.WriteLine("Opening Page");
            ccPage = e.NewElement as CreditCardEntryPage;

            // Launch the Card.IO activity as soon as we go into the renderer.
            Activity activity = this.Context as Activity;

            var intent = new Intent(activity, typeof(CardIOActivity));
            intent.PutExtra(CardIOActivity.ExtraRequireExpiry, ccPage.cardIOConfig.RequireExpiry);
            intent.PutExtra(CardIOActivity.ExtraRequireCvv, ccPage.cardIOConfig.RequireCvv);
            intent.PutExtra(CardIOActivity.ExtraRequirePostalCode, ccPage.cardIOConfig.RequirePostalCode);
            intent.PutExtra(CardIOActivity.ExtraHideCardioLogo, ccPage.cardIOConfig.HideCardIOLogo);
            intent.PutExtra(CardIOActivity.ExtraRequireCardholderName, ccPage.cardIOConfig.CollectCardholderName);
            intent.PutExtra(CardIOActivity.ExtraUsePaypalActionbarIcon, false);

            if (!string.IsNullOrEmpty(ccPage.cardIOConfig.Localization)) intent.PutExtra(CardIOActivity.ExtraLanguageOrLocale, ccPage.cardIOConfig.Localization);
            if (!string.IsNullOrEmpty(ccPage.cardIOConfig.ScanInstructions)) intent.PutExtra(CardIOActivity.ExtraScanInstructions, ccPage.cardIOConfig.ScanInstructions);

            activity.StartActivityForResult(intent, 101);
        }
    }
}
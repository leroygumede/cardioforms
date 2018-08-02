using Android.App;
using Android.Content;
using Card.IO;
using DrawIOForms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using System;
using Android.Runtime;

[assembly: ExportRenderer(typeof(CreditCardEntryPage), typeof(DrawIOForms.Droid.Renderers.CreditCardEntryPageRenderer))]
namespace DrawIOForms.Droid.Renderers
{
    public class ActivityResultEventArgs : EventArgs
    {
        public int RequestCode { get; set; }
        public Result ResultCode { get; set; }
        public Intent Data { get; set; }

        public ActivityResultEventArgs() : base()
        {

        }
    }

    public class CreditCardEntryPageRenderer : PageRenderer
    {
        private CreditCardEntryPage ccPage;

        public CreditCardEntryPageRenderer(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);
            var activity = Context as MainActivity;

            if (e.OldElement != null)
            {
                activity.ActivityResult -= Activity_ActivityResult;
            }
            if (e.NewElement != null)
            {
                activity.ActivityResult += Activity_ActivityResult;
            }

            if (e.OldElement != null || Element == null)
            {
                return;
            }

            Console.WriteLine("Opening Page");
            ccPage = e.NewElement as CreditCardEntryPage;

            // Launch the Card.IO activity as soon as we go into the renderer.

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

        void Activity_ActivityResult(object sender, ActivityResultEventArgs e)
        {
            if (e.Data != null)
            {
                var card = e.Data.GetParcelableExtra(CardIOActivity.ExtraScanResult).JavaCast<CreditCard>();
                Console.WriteLine($"Got result: {card.RedactedCardNumber}");
            }
        }
    }
}
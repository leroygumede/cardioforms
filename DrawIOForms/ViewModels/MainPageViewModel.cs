using Prism.Commands;
using Prism.Navigation;

namespace DrawIOForms.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public DelegateCommand ScanCardCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            _navigationService = navigationService;

            Title = "Main Page";
            ScanCardCommand = new DelegateCommand(OnClickScan);
        }

        public async void OnClickScan()
        {
            await _navigationService.NavigateAsync("CreditCardEntryPage");
        }
    }
}

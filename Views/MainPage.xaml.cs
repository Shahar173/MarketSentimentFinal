using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var vm = BindingContext as MainPageViewModel;
            if (vm != null)
            {
                vm.LoadDashboardData(); // קורא ומעדכן את המשתנים הסטטיים של החדשות והלווייתנים יחד
            }
        }
    }
}
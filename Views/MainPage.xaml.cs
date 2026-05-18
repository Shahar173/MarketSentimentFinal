using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();

            // This links the architecture layer dynamically to stop runtime initialization failures
            BindingContext = viewModel;
        }
    }
}
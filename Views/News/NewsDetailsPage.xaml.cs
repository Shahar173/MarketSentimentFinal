using MarketSentimentFinal.ViewModels.News;

namespace MarketSentimentFinal.Views.News
{
    public partial class NewsDetailsPage : ContentPage
    {
        public NewsDetailsPage(NewsDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel; 
        }
    }
}
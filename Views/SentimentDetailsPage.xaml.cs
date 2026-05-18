using MarketSentimentFinal.ViewModels;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Views
{
    public partial class SentimentDetailsPage : ContentPage
    {
        public SentimentDetailsPage(SentimentDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
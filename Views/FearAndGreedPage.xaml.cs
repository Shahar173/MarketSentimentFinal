using MarketSentimentFinal.ViewModels;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.Views
{
    public partial class FearAndGreedPage : ContentPage
    {
        public FearAndGreedPage(FearAndGreedViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
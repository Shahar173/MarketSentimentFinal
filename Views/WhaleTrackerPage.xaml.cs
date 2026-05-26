using Microsoft.Maui.Controls;
using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views
{
    public partial class WhaleTrackerPage : ContentPage
    {
        public WhaleTrackerPage(WhaleTrackerViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
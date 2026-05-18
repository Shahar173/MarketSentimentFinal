namespace MarketSentimentFinal.Views.News
{
    public partial class ViewNewsPage : ContentPage
    {
        public ViewNewsPage(ViewModels.News.ViewNewsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
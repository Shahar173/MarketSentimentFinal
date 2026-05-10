namespace MarketSentimentFinal.Views;

public partial class AdminPage : ContentPage
{
    public AdminPage(ViewModels.AdminPageViewModel viewModel)
    {
        InitializeComponent();
        // זה החלק הקריטי!
        BindingContext = viewModel;
    }
}
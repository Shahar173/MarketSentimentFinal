using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views;

public partial class UsersListPage : ContentPage
{
    public UsersListPage(UsersListViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}
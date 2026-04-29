using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views;

public partial class UserDetailsPage : ContentPage
{
    public UserDetailsPage(UserDetailsPageViewModel vm)
    {
        InitializeComponent();

        // זה החלק שחסר שגורם ל-XAML לא לזהות את ה-ViewModel
        BindingContext = vm;
    }
}
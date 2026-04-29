using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views;

public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel vm)
    {
        InitializeComponent();

        // חיבור ה-ViewModel לדף כדי שה-Binding יעבוד
        BindingContext = vm;
    }
}
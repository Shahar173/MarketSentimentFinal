using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views
{
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage(SignUpViewModel vm) // בדיוק כמו בלוגין
        {
            InitializeComponent();
            BindingContext = vm;
        }
    }
}
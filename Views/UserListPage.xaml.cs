using MarketSentimentFinal.ViewModels;

namespace MarketSentimentFinal.Views
{
    public partial class UsersListPage : ContentPage
    {
        public UsersListPage(UsersListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        // מתודה שמופעלת אוטומטית בכל פעם שהמסך עולה או חוזרים אליו
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // גישה ל-ViewModel וביצוע טעינה מחודשת וטרייה מה-Database
            if (BindingContext is UsersListViewModel viewModel)
            {
                await viewModel.LoadAllUsers();
            }
        }
    }
}
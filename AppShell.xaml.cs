using MarketSentimentFinal.ViewModels;
using MarketSentimentFinal.Views;

namespace MarketSentimentFinal
{
    public partial class AppShell : Shell
    {
        public AppShell(AppShellViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;

            // 1. הפיכת הסרגל לשקוף
            Shell.SetBackgroundColor(this, Colors.Transparent);
            Shell.SetTitleColor(this, Colors.White);

            // 2. התיקון הקריטי: העלמת הפס השחור בצד והרחבת ה-TitleView לכל הרוחב
            Microsoft.Maui.Handlers.ToolbarHandler.Mapper.AppendToMapping("CustomToolbar", (handler, view) =>
            {
#if ANDROID
                // מבטל את השוליים הפנימיים של ה-Toolbar ב-Android
                handler.PlatformView.SetContentInsetsAbsolute(0, 0);
                handler.PlatformView.ContentInsetStartWithNavigation = 0;
#endif
            });

            // רישום נתיבים
            Routing.RegisterRoute("SignUpPage", typeof(SignUpPage));
            Routing.RegisterRoute("LoginPage", typeof(LoginPage));
            Routing.RegisterRoute("MainPage", typeof(MainPage));
            Routing.RegisterRoute("UserDetailsPage", typeof(UserDetailsPage));
            Routing.RegisterRoute("AdminPage", typeof(AdminPage));
            Routing.RegisterRoute("UsersListPage", typeof(UsersListPage));
        }
    }
}
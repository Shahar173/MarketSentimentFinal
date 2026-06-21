using System;
using System.Windows.Input;
using MarketSentimentFinal.Models;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;

namespace MarketSentimentFinal.ViewModels.News
{
    [QueryProperty(nameof(Article), "SelectedArticle")]
    public class NewsDetailsViewModel : ViewModelBase
    {
        private NewsArticle _article;

        public NewsArticle Article
        {
            get => _article;
            set
            {
                _article = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenArticleCommand { get; }
        public ICommand GoBackCommand { get; }

        public NewsDetailsViewModel()
        {
            // פקודה מעודכנת וחסינה לפתיחת אתר החדשות המלא בדפדפן המכשיר
            OpenArticleCommand = new Command(async () =>
            {
                // בדיקה ראשונית - הצגת הקישור הקיים כדי להבין אם הוא null או פשוט שגוי
                if (Article == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Article object is entirely null.", "OK");
                    return;
                }

                if (string.IsNullOrWhiteSpace(Article.ArticleUrl))
                {
                    // הפופ-אפ המעודכן יראה לך בדיוק מה ערך הקישור הריק
                    await Shell.Current.DisplayAlert("Debug Info", $"URL value is empty. Title: {Article.Title}", "OK");
                    return;
                }

                try
                {
                    string url = Article.ArticleUrl.Trim();

                    if (!url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
                        !url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                    {
                        url = "https://" + url;
                    }

                    await Launcher.Default.OpenAsync(new Uri(url));
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Error", $"Could not open link: {ex.Message}", "OK");
                }
            });

            // ניווט לאחור וניקוי ה-Stack
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;
using Microsoft.Maui.Controls;

namespace MarketSentimentFinal.ViewModels.News
{
    public class ViewNewsViewModel : ViewModelBase
    {
        private readonly INewsService _newsService;
        private bool _isLoading;

        // משתנה סטטי גלובלי שיחזיק את ציון הסנטימנט של החדשות עבור ה-MainPage
        public static int SharedNewsScore { get; set; } = 50; // ברירת מחדל ניטרלי

        public ObservableCollection<NewsArticle> NewsList { get; } = new();

        public bool IsLoading
        {
            get => _isLoading;
            set { _isLoading = value; OnPropertyChanged(); }
        }

        public ICommand FetchNewsCommand { get; }
        public ICommand GoToDetailsCommand { get; }
        public ICommand GoBackCommand { get; }

        public ViewNewsViewModel(INewsService newsService)
        {
            _newsService = newsService;

            FetchNewsCommand = new Command(async () => await LoadNewsAsync());

            GoToDetailsCommand = new Command<NewsArticle>(async (article) =>
            {
                if (article == null) return;
                var navParams = new Dictionary<string, object> { { "SelectedArticle", article } };
                await Shell.Current.GoToAsync("NewsDetailsPage", navParams);
            });

            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));

            // הפעלת טעינה ראשונית של החדשות כדי שיהיה ציון מוכן ב-Startup
            _ = LoadNewsAsync();
        }

        private async Task LoadNewsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                var articles = await _newsService.GetCryptoNewsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    NewsList.Clear();

                    if (articles != null && articles.Any())
                    {
                        foreach (var article in articles)
                        {
                            NewsList.Add(article);
                        }

                        // חישוב הסנטימנט מתוך הכתבות שחזרו
                        CalculateNewsSentiment(articles.ToList());
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine("Service returned null or empty list.");
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading news: {ex.Message}");
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() => IsLoading = false);
            }
        }

        private void CalculateNewsSentiment(List<NewsArticle> articles)
        {
            if (articles == null || !articles.Any()) return;

            int positiveCount = 0; // יספור כתבות BULLISH
            int negativeCount = 0; // יספור כתבות BEARISH

            foreach (var article in articles)
            {
                // המרה לאותיות קטנות כדי למנוע בעיות של רווחים או אותיות גדולות/קטנות
                string sentiment = article.Sentiment?.ToLower()?.Trim() ?? "neutral";

                if (sentiment == "bullish")
                {
                    positiveCount++;
                }
                else if (sentiment == "bearish")
                {
                    negativeCount++;
                }
            }

            int totalPolarized = positiveCount + negativeCount;

            if (totalPolarized > 0)
            {
                // חישוב יחס הכתבות השוריות (Bullish) מתוך סך הכל הכתבות המקוטבות
                double positiveRatio = (double)positiveCount / totalPolarized;
                SharedNewsScore = (int)(positiveRatio * 100);
            }
            else
            {
                SharedNewsScore = 50; // ברירת מחדל אם הכל ניטרלי
            }

            // עדכון כפוי ודינמי של ה-UI ב-MainPage ב-Main Thread
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (Application.Current?.MainPage is Shell shell && shell.CurrentPage?.BindingContext is MainPageViewModel mainVM)
                {
                    mainVM.LoadDashboardData();
                }
            });

            System.Diagnostics.Debug.WriteLine($"[NEWS SENTIMENT FIXED] Score: {SharedNewsScore} (Bullish: {positiveCount}, Bearish: {negativeCount})");
        }
    }
}
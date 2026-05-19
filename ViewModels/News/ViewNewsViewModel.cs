using System.Collections.ObjectModel;
using System.Windows.Input;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services;

namespace MarketSentimentFinal.ViewModels.News
{
    public class ViewNewsViewModel : ViewModelBase
    {
        private readonly INewsService _newsService;
        private bool _isLoading;

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

            // Using // to force a push-animation back to root
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync("//MainPage"));
        }

        private async Task LoadNewsAsync()
        {
            if (IsLoading) return;

            IsLoading = true;

            try
            {
                var articles = await _newsService.GetCryptoNewsAsync();

                // Ensure we interact with the ObservableCollection on the Main Thread
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    NewsList.Clear();

                    if (articles != null && articles.Any())
                    {
                        foreach (var article in articles)
                        {
                            NewsList.Add(article);
                        }
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
                // Ensure IsLoading is reset on Main Thread to stop any UI activity indicators
                MainThread.BeginInvokeOnMainThread(() => IsLoading = false);
            }
        }
    }
}
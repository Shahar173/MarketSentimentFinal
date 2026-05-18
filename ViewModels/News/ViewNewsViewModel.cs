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

        public ObservableCollection<NewsArticle> NewsList { get; set; } = new();

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

            // Fixed: Push to NewsDetailsPage as a relative sub-route stack append 
            GoToDetailsCommand = new Command<NewsArticle>(async (article) =>
            {
                if (article == null) return;
                var navParams = new Dictionary<string, object> { { "SelectedArticle", article } };
                await Shell.Current.GoToAsync("NewsDetailsPage", navParams);
            });

            // Fixed: Reset cleanly back out to the main dashboard module layer
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }

        private async Task LoadNewsAsync()
        {
            if (IsLoading) return;

            MainThread.BeginInvokeOnMainThread(() =>
            {
                IsLoading = true;
                NewsList.Clear();
            });

            try
            {
                var articles = await _newsService.GetCryptoNewsAsync();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    if (articles != null)
                    {
                        foreach (var article in articles)
                        {
                            NewsList.Add(article);
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading news UI: {ex.Message}");
            }
            finally
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    IsLoading = false;
                });
            }
        }
    }
}
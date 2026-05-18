using System.Windows.Input;
using MarketSentimentFinal.Models;

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
            // Opens the full news website in the system browser
            OpenArticleCommand = new Command(async () =>
            {
                if (Article != null && !string.IsNullOrEmpty(Article.ArticleUrl))
                {
                    await Launcher.Default.OpenAsync(new Uri(Article.ArticleUrl));
                }
            });

            // Native Pop: Strips the details page off the stack cleanly 
            // without destroying or resetting the underlying news feed data state.
            GoBackCommand = new Command(async () => await Shell.Current.GoToAsync(".."));
        }
    }
}
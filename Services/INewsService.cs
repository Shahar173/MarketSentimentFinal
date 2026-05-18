using MarketSentimentFinal.Models;

namespace MarketSentimentFinal.Services
{
    public interface INewsService
    {
        // This will fetch the latest news and return our list of models
        Task<List<NewsArticle>> GetCryptoNewsAsync();
    }
}
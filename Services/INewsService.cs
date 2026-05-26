using MarketSentimentFinal.Models;

namespace MarketSentimentFinal.Services
{
    // ממשק המגדיר חוזה עבודה לכל מחלקה שמטפלת במשיכת חדשות מהרשת
    public interface INewsService
    {
        Task<List<NewsArticle>> GetCryptoNewsAsync();
    }
}
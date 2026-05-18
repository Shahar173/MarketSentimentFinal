using Microsoft.Maui.Graphics;

namespace MarketSentimentFinal.Models
{
    public class NewsArticle
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string PublishedAt { get; set; } = string.Empty;
        public string ArticleUrl { get; set; } = string.Empty;

        // AI Sentiment Data
        public string Sentiment { get; set; } = "NEUTRAL"; // BULLISH, BEARISH, NEUTRAL
        public double SentimentScore { get; set; }

        // UI Color
        public Color SentimentColor { get; set; } = Colors.Gray;
    }
}
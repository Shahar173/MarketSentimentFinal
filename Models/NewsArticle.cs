using System;
using System.Text.Json.Serialization;
using Microsoft.Maui.Graphics;

namespace MarketSentimentFinal.Models
{
    public class NewsArticle
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; } = string.Empty;

        // תוקן: מיפוי השדה "text" מה-API לתוך המאפיין Description שלך
        [JsonPropertyName("text")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("source_name")]
        public string Source { get; set; } = string.Empty;

        [JsonPropertyName("date")]
        public string PublishedAt { get; set; } = string.Empty;

        [JsonPropertyName("news_url")]
        public string ArticleUrl { get; set; } = string.Empty;

        public string Sentiment { get; set; } = "NEUTRAL";
        public double SentimentScore { get; set; }

        [JsonIgnore]
        public Color SentimentColor { get; set; } = Colors.Gray;
    }
}
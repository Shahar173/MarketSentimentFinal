using System;

namespace MarketSentimentFinal.Models
{
    public class NewsArticle
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;

        // סנטימנט: BULLISH, BEARISH, NEUTRAL
        public string Sentiment { get; set; } = "NEUTRAL";

        // צבע התגית שיופיע ב-UI
        public Color SentimentColor { get; set; } = Colors.Gray;

        public string PublishedAt { get; set; } = string.Empty;
    }
}
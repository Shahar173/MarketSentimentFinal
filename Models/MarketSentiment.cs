using System;

namespace MarketSentimentFinal.Models
{
    public class MarketSentiment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int FearAndGreedIndex { get; set; } // 0-100
        public string OverallMarketTrend { get; set; } = "Stable";
        public double AverageSentimentScore { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.Now;
    }
}
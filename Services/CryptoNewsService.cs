using System.Text.Json;
using MarketSentimentFinal.Models;

namespace MarketSentimentFinal.Services
{
    public class CryptoNewsService : INewsService
    {
        private readonly HttpClient _httpClient;

        public CryptoNewsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        public async Task<List<NewsArticle>> GetCryptoNewsAsync()
        {
            var articles = new List<NewsArticle>();
            try
            {
                // Using a 100% open, public rss-to-json converter feed for CoinTelegraph live breaking news
                string url = "https://api.rss2json.com/v1/api.json?rss_url=https%3A%2F%2Fcointelegraph.com%2Frss";

                System.Diagnostics.Debug.WriteLine($"[NEWS API] Switching feed source to: {url}");
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(jsonString);

                    // The open RSS API returns a root object with an "items" array inside
                    if (document.RootElement.TryGetProperty("items", out var itemsArray))
                    {
                        int count = 0;
                        foreach (var item in itemsArray.EnumerateArray().Take(20))
                        {
                            var newsArticle = new NewsArticle
                            {
                                Title = item.TryGetProperty("title", out var t) ? t.GetString() : "No Title",
                                // Grabbing news content or description layout text
                                Description = item.TryGetProperty("description", out var d) ? StripHtmlTags(d.GetString()) : "No Description",
                                Source = "CoinTelegraph",
                                ArticleUrl = item.TryGetProperty("link", out var l) ? l.GetString() : "",
                                Sentiment = "NEUTRAL",
                                SentimentColor = Color.FromArgb("#95A5A6"),
                                PublishedAt = item.TryGetProperty("pubDate", out var p) ? p.GetString() : "Just now"
                            };

                            articles.Add(newsArticle);
                            count++;
                        }
                        System.Diagnostics.Debug.WriteLine($"[NEWS API] Success! Loaded {count} breaking crypto articles.");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[NEWS API] Server error status code: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[NEWS API] Error building objects: {ex.Message}");
            }

            return articles;
        }

        // Helper method to clean up any raw HTML tags inside standard RSS descriptions
        private string StripHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input)) return "No Description";
            // Simple clean to clear out image tags or paragraph tags from the feed text
            var cleanText = System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", String.Empty);
            return cleanText.Length > 180 ? cleanText.Substring(0, 175) + "..." : cleanText;
        }
    }
}
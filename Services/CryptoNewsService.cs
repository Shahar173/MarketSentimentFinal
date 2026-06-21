using System.Text.Json;
using MarketSentimentFinal.Models;

namespace MarketSentimentFinal.Services
{
    public class CryptoNewsService : INewsService
    {
        private readonly HttpClient _httpClient;

        // קבוצה 1: הגדרות התחברות (HttpClient ו-API Token)
        private readonly string _apiKey = "2ke2ezrpzznixlsh44l96dl5ivrxcfl31lhubxwd";

        public CryptoNewsService()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");
        }

        // קבוצה 2: לוגיקת המשיכה והעיבוד מה-API
        public async Task<List<NewsArticle>> GetCryptoNewsAsync()
        {
            var articles = new List<NewsArticle>();
            try
            {
                string url = $"https://cryptonews-api.com/api/v1/category?section=alltickers&items=20&token={_apiKey}";

                System.Diagnostics.Debug.WriteLine($"[NEWS API] Fetching premium news from: {url}");
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();
                    using var document = JsonDocument.Parse(jsonString);

                    if (document.RootElement.TryGetProperty("data", out var itemsArray))
                    {
                        int count = 0;
                        foreach (var item in itemsArray.EnumerateArray())
                        {
                            string rawSentiment = item.TryGetProperty("sentiment", out var s) ? s.GetString().ToUpper() : "NEUTRAL";
                            string sentiment = rawSentiment switch
                            {
                                "POSITIVE" => "BULLISH",
                                "NEGATIVE" => "BEARISH",
                                _ => "NEUTRAL"
                            };
                            string rawDate = item.TryGetProperty("date", out var p) ? p.GetString() : "Just now";
                            string israelFormattedDate = ConvertToIsraelTime(rawDate);

                            var newsArticle = new NewsArticle
                            {
                                Title = item.TryGetProperty("title", out var t) ? t.GetString() : "No Title",
                                Description = item.TryGetProperty("text", out var d) ? StripHtmlTags(d.GetString()) : "No Description",
                                Source = item.TryGetProperty("source_name", out var src) ? src.GetString() : "Crypto News",
                                ArticleUrl = item.TryGetProperty("news_url", out var l) ? l.GetString() : "",
                                PublishedAt = israelFormattedDate,

                                Sentiment = sentiment,
                                SentimentScore = 0.0,
                                SentimentColor = GetSentimentColor(sentiment)
                            };

                            articles.Add(newsArticle);
                            count++;
                        }
                        System.Diagnostics.Debug.WriteLine($"[NEWS API] Success! Loaded {count} premium crypto articles.");
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

        // קבוצה 3: מתודות עזר (המרה לזמן ישראל, צבעים וניקוי טקסט)
        private string ConvertToIsraelTime(string rawDate)
        {
            if (DateTimeOffset.TryParse(rawDate, out var dateTimeOffset))
            {
                TimeZoneInfo israelZone;
                try
                {
                    israelZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Jerusalem");
                }
                catch
                {
                    try
                    {
                        israelZone = TimeZoneInfo.FindSystemTimeZoneById("Israel Standard Time");
                    }
                    catch
                    {
                        israelZone = TimeZoneInfo.Local;
                    }
                }

                var israelTime = TimeZoneInfo.ConvertTime(dateTimeOffset, israelZone);

                return israelTime.ToString("ddd, dd MMM yyyy HH:mm:ss");
            }

            return rawDate;
        }

        private Color GetSentimentColor(string sentiment)
        {
            return sentiment switch
            {
                "BULLISH" => Color.FromArgb("#2ECC71"),
                "BEARISH" => Color.FromArgb("#E74C3C"),
                _ => Color.FromArgb("#95A5A6")
            };
        }

        private string StripHtmlTags(string input)
        {
            if (string.IsNullOrEmpty(input)) return "No Description";
            var cleanText = System.Text.RegularExpressions.Regex.Replace(input, "<.*?>", String.Empty);
            return cleanText.Length > 180 ? cleanText.Substring(0, 175) + "..." : cleanText;
        }
    }
}
using Microsoft.Maui.Graphics;

namespace MarketSentimentFinal.Models
{
    public class NewsArticle
    {
        // קבוצה 1: מידע בסיסי על הכתבה שמגיע מה-API (כותרת, תיאור, מקור וקישור)
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public string PublishedAt { get; set; } = string.Empty;
        public string ArticleUrl { get; set; } = string.Empty;

        // קבוצה 2: נתוני הניתוח של ה-AI (סיווג הסנטימנט ועוצמת הציון)
        public string Sentiment { get; set; } = "NEUTRAL";
        public double SentimentScore { get; set; }

        // קבוצה 3: נתון עיצובי (צבע הרקע של הכרטיס במסך בהתאם לסנטימנט)
        public Color SentimentColor { get; set; } = Colors.Gray;
    }
}
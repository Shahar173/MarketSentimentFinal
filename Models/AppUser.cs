using System;

namespace MarketSentimentFinal.Models
{
    public class AppUser
    {
        // מזהה ייחודי ל-Firebase
        public string Id { get; set; } = string.Empty;

        // פרטים אישיים
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        // השדה שהיה חסר וגרם לשגיאות
        public string Mobile { get; set; } = string.Empty;

        // פרטי התחברות
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        // ניהול ומעקב
        public bool IsAdmin { get; set; } = false;
        public DateTime RegDate { get; set; } = DateTime.Now;
        public DateTime LastLogin { get; set; } = DateTime.Now;
    }
}
using System;
using Firebase.Database; // אם זה אדום אחרי ה-Rebuild, ה-csproj לא נטען טוב
using MarketSentimentFinal.Services.DBService; // וודא שזה הנתיב לממשק שלך

namespace MarketSentimentFinal.Services
{
    public class FirebaseRealtimeService : IDbInstance
    {
        protected FirebaseClient? _firebaseClient;

        public FirebaseRealtimeService()
        {
            _firebaseClient = new FirebaseClient("https://fir-class-28e82-default-rtdb.europe-west1.firebasedatabase.app/");
        }
        public string Info()
        {
            return "Type: Google Firebase RealTime Database client";
        }
    }
}
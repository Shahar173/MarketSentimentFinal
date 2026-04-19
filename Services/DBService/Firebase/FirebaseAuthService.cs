using Firebase.Auth;
using Firebase.Auth.Providers;
using MarketSentimentFinal.Services.DBService;
using System;
using System.Threading.Tasks;

// פישטתי את ה-Namespace כדי שיהיה לך קל לרשום אותו ב-MauiProgram
namespace MarketSentimentFinal.Services
{
    public class FirebaseAuthService : IAuthService
    {
        private readonly FirebaseAuthClient _authClient;

        public FirebaseAuthService()
        {
            // הגדרות החיבור לפיירבייס שלך
            var config = new FirebaseAuthConfig
            {
                ApiKey = "AIzaSyDgOIkrlszkwf5GQpPymAHpaeAqoP5ct9k",
                AuthDomain = "fir-class-28e82.firebaseapp.com",
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            _authClient = new FirebaseAuthClient(config);
        }

        public async Task<string> SignIn(string userEmail, string userPassword)
        {
            try
            {
                var userCredential = await _authClient.SignInWithEmailAndPasswordAsync(userEmail, userPassword);
                // מחזיר את ה-UID של המשתמש (מזהה ייחודי)
                return userCredential.User.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                // כאן נתפוס שגיאות ספציפיות של פיירבייס
                string errorMessage = ex.Reason switch
                {
                    AuthErrorReason.InvalidEmailAddress => "כתובת אימייל לא תקינה",
                    AuthErrorReason.WrongPassword => "סיסמה שגויה",
                    AuthErrorReason.UserNotFound => "משתמש לא קיים",
                    _ => "שגיאה בהתחברות: " + ex.Message
                };
                throw new Exception(errorMessage);
            }
        }

        public async Task<string> CreateAuth(string userEmail, string userPassword)
        {
            try
            {
                var userCredential = await _authClient.CreateUserWithEmailAndPasswordAsync(userEmail, userPassword);
                return userCredential.User.Uid;
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = ex.Reason switch
                {
                    AuthErrorReason.EmailExists => "האימייל כבר קיים במערכת",
                    AuthErrorReason.InvalidEmailAddress => "כתובת אימייל לא תקינה",
                    AuthErrorReason.WeakPassword => "הסיסמה חלשה מדי",
                    _ => "שגיאה בהרשמה"
                };
                throw new Exception(errorMessage);
            }
        }

        public async Task SignOut()
        {
            _authClient.SignOut();
            await Task.CompletedTask;
        }

        // מימוש זמני לפונקציות שכרגע לא בשימוש כדי שהקוד יתקמפל
        public Task RemoveAuth(string userEmail, string userPassword) => Task.CompletedTask;
    }
}   
using Firebase.Database;
using Firebase.Database.Query;
using MarketSentimentFinal.Models;
using MarketSentimentFinal.Services.DBService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarketSentimentFinal.Services
{
    public class FirebaseUsersRepository : FirebaseRealtimeService, IAppUserRepository
    {
        private readonly IAuthService _authService;

        public FirebaseUsersRepository(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task<string> CreateAsync(AppUser appUser)
        {
            // שימוש ב-Email ו-Password מהמודל החדש שלך
            string userId = await _authService.CreateAuth(appUser.Email, appUser.Password);
            appUser.Id = userId;
            await _firebaseClient.Child("users").Child(userId).PutAsync(appUser);
            return userId;
        }

        public async Task<AppUser> SignInAsync(string userEmail, string userPassword)
        {
            string userId = await _authService.SignIn(userEmail, userPassword);
            return await GetUserByIdAsync(userId);
        }

        public async Task<AppUser> GetUserByIdAsync(string userId)
        {
            return await _firebaseClient.Child("users").Child(userId).OnceSingleAsync<AppUser>();
        }

        // כאן התיקון הקריטי לשגיאה CS0738
        public async Task<List<AppUser>> GetAllAsync()
        {
            var users = await _firebaseClient.Child("users").OnceAsync<AppUser>();
            return users.Select(u => u.Object).ToList();
        }

        public async Task UpdateAsync(AppUser appUser)
        {
            await _firebaseClient.Child("users").Child(appUser.Id).PutAsync(appUser);
        }

        public async Task DeleteAsync(AppUser appUser)
        {
            await _firebaseClient.Child("users").Child(appUser.Id).DeleteAsync();
        }

        public async Task SetToAdmin(string userId)
        {
            await _firebaseClient.Child("users").Child(userId).Child("IsAdmin").PutAsync(true);
        }
    }
}
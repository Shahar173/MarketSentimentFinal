using MarketSentimentFinal.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MarketSentimentFinal.Services
{
    public interface IAppUserRepository
    {
        Task<string> CreateAsync(AppUser appUser);
        Task UpdateAsync(AppUser appUser);
        Task DeleteAsync(AppUser appUser);
        Task<AppUser> SignInAsync(string userEmail, string userPassword);
        Task<AppUser> GetUserByIdAsync(string userId);
        // שנה את השורה הזו מ-List ל-Task<List>
        Task<List<AppUser>> GetAllAsync();
        Task SetToAdmin(string userId);
    }
}
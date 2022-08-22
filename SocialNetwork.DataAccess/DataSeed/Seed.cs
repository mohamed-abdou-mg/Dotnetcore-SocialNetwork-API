using Microsoft.EntityFrameworkCore;
using SocialNetwork.Models;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocialNetwork.DataAccess.DataSeed
{
    public class Seed
    {
        public static async Task SeedUsers(ApplicationDbContext dbContext)
        {
            if (await dbContext.Users.AnyAsync()) return;

            var usersData = await System.IO.File.ReadAllTextAsync("./UserSeedData.json");

            var users = JsonSerializer.Deserialize<List<AppUser>>(usersData);

            foreach (var user in users)
            {
                using var hmac = new HMACSHA512();
                user.Username = user.Username.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("2244mmMM@"));
                user.PasswordSalt = hmac.Key;
                dbContext.Users.Add(user);
            }
             
            await dbContext.SaveChangesAsync();
        }
    }
}

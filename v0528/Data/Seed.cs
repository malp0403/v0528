using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using v0528.Entities;

namespace v0528.Data
{
    public class Seed
    {
        public static async Task SeedUsers(DataContext context)
        {
            if (await context.Users.AnyAsync()) return;

            try
            {
                var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
                var users = JsonSerializer.Deserialize<List<AppUser>>(userData);
                foreach (var user in users)
                {
                    using var hmac = new HMACSHA512();
                    user.UserName = user.UserName.ToLower();
                    user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("password"));
                    user.PasswordSalt = hmac.Key;

                    context.Users.Add(user);
                }

                await context.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                var a = "";
            }

        }
    }
}

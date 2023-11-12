using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebStore.Constants;
using WebStore.Data.Entities;
using WebStore.Data.Entities.Identity;

namespace WebStore.Data
{
    public static class SeederDB
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices
                    .GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppEFContext>();
                context.Database.Migrate();

                var userManager = scope.ServiceProvider
                    .GetRequiredService<UserManager<UserEntity>>();

                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<RoleEntity>>();

                if (!context.Roles.Any())
                {
                    RoleEntity admin = new RoleEntity
                    {
                        Name = Roles.Admin,
                    };
                    RoleEntity user = new RoleEntity
                    {
                        Name = Roles.User,
                    };
                    var result = roleManager.CreateAsync(admin).Result;
                    result = roleManager.CreateAsync(user).Result;
                }

                if (!context.Users.Any())
                {
                    UserEntity user = new UserEntity
                    {
                        FirstName = "Марко",
                        LastName = "Муха",
                        Email = "muxa@gmail.com",
                        UserName = "muxa@gmail.com",
                    };
                    var result = userManager.CreateAsync(user, "123456")
                        .Result;
                    if (result.Succeeded)
                    {
                        result = userManager
                            .AddToRoleAsync(user, Roles.Admin)
                            .Result;
                    }
                }

                if (!context.Categories.Any())
                {
                    var laptop = new CategoryEntity
                    {
                        Name = "Ноутбуки",
                        Image= SaveUrlImage("https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE4OXzi?ver=3a58"),
                        UserId= 1,
                        Description="Для роботи і навчання",
                        DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    };
                    var clothes = new CategoryEntity
                    {
                        Name = "Одяг",
                        Image = SaveUrlImage("https://static.independent.co.uk/s3fs-public/thumbnails/image/2019/03/27/11/woman-clothing-happy.jpg?width=1200"),
                        UserId = 1,
                        Description = "Для дівчат і хлопців",
                        DateCreated = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc)
                    };
                    context.Categories.Add(laptop);
                    context.Categories.Add(clothes);
                    context.SaveChanges();
                }
            }
        }

        private static string SaveUrlImage(string url)
        {
            string imageName = string.Empty;
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    byte[] imageBytes = client.GetByteArrayAsync(url).Result;
                    imageName = Path.GetRandomFileName() + ".jpg";
                    string dirSaveImage = Path.Combine(Directory.GetCurrentDirectory(), "images", imageName);
                    // Save the downloaded image bytes to a file
                    File.WriteAllBytes(dirSaveImage, imageBytes);
                    return imageName;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error downloading or saving image: {ex.Message}");
                    return null;
                }
            }
        }
    }
}

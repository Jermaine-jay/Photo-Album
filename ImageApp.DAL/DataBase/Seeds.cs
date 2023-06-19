using ImageApp.DAL.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ImageApp.DAL.DataBase
{
    public class Seeds
    {
        public static async Task EnsurePopulatedAsync(IApplicationBuilder app)
        {
            ImageAppDbContext context = app.ApplicationServices.CreateScope().ServiceProvider
                .GetRequiredService<ImageAppDbContext>();

            if (!await context.Pictures.AnyAsync())
            {
                await context.Pictures.AddRangeAsync(AddPictures());
                await context.Pictures.AddRangeAsync(AddPictures2());
                await context.SaveChangesAsync();
            }
        }


        public static IEnumerable<Picture> AddPictures()
        {
            return new List<Picture>()
            {
                new Picture()
                {
                    Name = "Holiday picture",
                    Description = "Went to the woods in november 2018",
                    ImageFile = "Eel and heron.jpg",
                    UserId = "ee6875fd-0bbc-42e4-893c-88860a8d789e"
                },

                new Picture()
                {
                    Name = "Summer picnic park picture",
                    Description = "Went on a picnic in january 1st 2019, with my friends",
                    ImageFile = "Bronze-art-silva.jpg",
                    UserId = "ee6875fd-0bbc-42e4-893c-88860a8d789e"

                },

                new Picture()
                {
                    Name = "Skating on ice",
                    Description = "went for ice skating in april 2019 with my dad",
                    ImageFile = "1966 Ford Mustang Fastback C-Code.jpg",
                    UserId = "ee6875fd-0bbc-42e4-893c-88860a8d789e"

                },

            };
            
        }

        public static IEnumerable<Picture> AddPictures2()
        {
            return new List<Picture>()
            {

                new Picture()
                {
                    Name = "Playing football in spring",
                    Description = "went to play football in april 2020 with my friends",
                    ImageFile = "1990 Porsche 964 Carrera Targa.jpg",
                    UserId = "332185f8-0583-427a-bf97-4eac880ea40c"

                },

                new Picture()
                {
                    Name = "Random picture",
                    Description = "Took some random picture with my friends in campus(2017)",
                    ImageFile = "1987 BMW 325E.jpg",
                    UserId = "332185f8-0583-427a-bf97-4eac880ea40c"

                },

                new Picture()
                {
                    Name = "St Anthony's harvest 2017",
                    Description = "Harvest celebration before covid 19",
                    ImageFile = "Bronze-art-silva.jpg",
                    UserId = "332185f8-0583-427a-bf97-4eac880ea40c"

                },

            };
            
        }
    }
}

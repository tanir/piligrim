using System;
using System.Collections.Generic;
using System.Linq;
using Piligrim.Core;
using Piligrim.Core.Models;

namespace Piligrim.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ProductsDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Products.Any())
            {
                return;
            }
            var rnd = new Random();
            
            var allCategories = new HashSet<string>(AvailableCategories.Categories.Select(x => x.Name));

            allCategories.UnionWith(AvailableCategories.Categories.SelectMany(x => x.Child).Select(x => x.Name));

            for (var i = 0; i < 50; i++)
            {
                context.Products.Add(new Product
                {
                    Title = $"Продукция {i}",
                    Category = allCategories.OrderBy(x => Guid.NewGuid()).First(),
                    Colors = new List<Color>
                    {
                        new Color {Value = i % 2 == 0 ? "Желтый" : "Зеленый"},
                        new Color {Value = i % 3 == 0 ? "Оранжевый" : "Синий"},
                        new Color {Value = i % 4 == 0 ? "Кораловый" : "Красный"}
                    },
                    Description =
                        $"Это описание продукции {i}. Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.",
                    Price = (decimal)(rnd.Next(10, 5000) + rnd.NextDouble()),
                    Sizes = new List<Size>
                    {
                        new Size {Value = "200x300"},
                        new Size {Value = "300x200"},
                        new Size {Value = "400x400"}
                    },
                    Photos = new List<Photo>
                    {
                        new Photo {Uri = $"http://lorempixel.com/1024/768/nature/?{Guid.NewGuid()}"}
                    },
                    Thumbnail = $"http://lorempixel.com/1024/768/nature/?{Guid.NewGuid()}"
                });
            }

            context.SaveChanges();
        }
    }
}

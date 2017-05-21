using System.Collections.Generic;
using System.Linq;

namespace Piligrim.Core.Categories
{
    public class StaticCategoriesProvider : ICategoriesProvider
    {
        public static IEnumerable<Category> Categories { get; } = new[]
        {
            new Category("furnitura-dlya-remonta-chemodanov", "Фурнитура для ремонта чемоданов и сумок")
            {
                Child = new[]
                {
                    new Category("kolesa", "Колеса, система колес"),
                    new Category("ruchki", "Ручки"),
                    new Category("zamok-dlya-chemodanov", "Замок для чемоданов"),
                    new Category("Podshipniki", "Подшипники, направляющие для колес"),
                    new Category("vidvizhnaya-sistema", "Выдвижная система"),
                    new Category("nozhki", "Ножки, подставки для чемоданов"),
                    new Category("begunok", "Бегунок"),
                    new Category("molniya", "Молния")
                }
            },
            new Category("Naboiki", "Набойки")
            {
                Child = new[] {new Category("muzhskie", "Мужские"), new Category("zhenskie", "Женские")}
            },
            new Category("listovoy-poliuretan", "Листовой полиуретан"),
            new Category("klin-pod-naboiku", "Клин под набойку"),
            new Category("podmetki-profilaktika", "Подметки-профилактика"),
            new Category("nitki", "Нитки"),
            new Category("rezina", "Резина")
        };


        public IEnumerable<Category> GetAll()
        {
            return Categories;
        }

        public Category Get(string categoryName)
        {
            return Categories.SelectMany(x => x.Child).Union(Categories).FirstOrDefault(x => x.Name == categoryName);
        }

        public IEnumerable<Category> Leafs()
        {
            return Categories
                .SelectMany(x => x.Child)
                .Union(Categories)
                .Where(x => x.Child == null || !x.Child.Any())
                .ToList();
        }
    }
}
using System.Collections.Generic;

namespace Piligrim.Core.Categories
{
    public interface ICategoriesProvider
    {
        IEnumerable<Category> GetAll();

        Category Get(string categoryName);

        IEnumerable<Category> Leafs();
    }
}
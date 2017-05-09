﻿using System.Collections.Generic;

namespace Piligrim.Web.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public string Title { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public IEnumerable<string> Photos { get; set; }

        public string Thumbnail { get; set; }

        public IEnumerable<string> Sizes { get; set; }

        public IEnumerable<string> Colors { get; set; }
    }
}

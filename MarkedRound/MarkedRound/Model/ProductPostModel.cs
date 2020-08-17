using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarkedRound.Model
{
    public class ProductPostModel
    {
        public ProductPostModel(string section, ProductPost_Input input)
        {
            this.section = section;
            this.input = input;
        }

        public string section { get; set; }
        public ProductPost_Input input { get; set; }
        public AdvancedSearchModel AdvancedSearch { get; set; }
    }

    public class ProductPost_Input
    {
        public ProductPost_Input(string content)
        {
            this.search = search;
        }

        public string search { get; set; }
    }

    public class AdvancedSearchModel
    {
        public AdvancedSearchModel(int priceRangeLow, int priceRangeHigh, bool priceRange, bool highToLow, bool lowToHigh, bool alphabeticalOrder, bool showNewest)
        {
            PriceRangeLow = priceRangeLow;
            PriceRangeHigh = priceRangeHigh;
            PriceRange = priceRange;
            HighToLow = highToLow;
            LowToHigh = lowToHigh;
            AlphabeticalOrder = alphabeticalOrder;
            ShowNewest = showNewest;
        }

        public int PriceRangeLow { get; set; }
        public int PriceRangeHigh { get; set; }
        public bool PriceRange { get; set; }

        public bool HighToLow { get; set; }
        public bool LowToHigh { get; set; }
        public bool AlphabeticalOrder { get; set; }
        public bool ShowNewest { get; set; }
    }
}

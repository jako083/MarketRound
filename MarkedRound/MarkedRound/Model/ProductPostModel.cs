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
    }

    public class ProductPost_Input
    {
        public ProductPost_Input(string content)
        {
            this.search = search;
        }

        public string search { get; set; }
    }
}

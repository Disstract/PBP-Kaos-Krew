using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaosKrewScraper
{
    public class PostFetchingClass
    {
        public string? Title { get; set; }
        public List<string>? URL1 { get; set; }
        public List<string>? URL2 { get; set; }
        public List<string>? URL3 { get; set; }
        public List<string>? URL4 { get; set; }
    }

    public class DecryptorClass
    {
        public bool? success { get; set; }
        public List<string>? data { get; set; }
        public string? message { get; set; }
    }
}

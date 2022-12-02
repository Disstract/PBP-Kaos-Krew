using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaosPrefetch
{
    public class FetchQuery
    {
        public string? Title { get; set; }
        public string? URL { get; set; }
    }

    public class QueryList
    {
        public List<FetchQuery>? response { get; set; }
    }
}

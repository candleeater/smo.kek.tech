using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Api.v1
{
    public class QueryField
    {
        public string Name { get; set; }
        public bool IsSearchable { get; set; }
        public bool IsList { get; set; }
    }
}

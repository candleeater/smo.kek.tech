using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class QueryFieldAttribute: Attribute
    {
        public string Name { get; set; }
        public bool IsSearchable { get; set; } = false;
        public bool IsList { get; set; } = false;
    }
}

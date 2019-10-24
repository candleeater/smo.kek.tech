using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class QueryClassAttribute : Attribute
    {
        public string Alias { get; set; } = null;
        public string SqlKey { get; set; } = null;
        public string SqlCountKey { get; set; } = null;
    }
}

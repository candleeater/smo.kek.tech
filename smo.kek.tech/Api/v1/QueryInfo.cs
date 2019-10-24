using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Api.v1
{
    public class QueryInfo
    {
        public List<string> Filters { get; set; } = new List<string>() { "1 = 1" };
        public List<string> Columns { get; set; } = new List<string>();
        public Dictionary<string, object> Parameters = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public bool IsPaged { get; set; } = false;
        public bool Randomize { get; set; } = false;
        public int Seed { get; set; }

        #region Computed

        public string FilterString
        {
            get
            {
                return string.Join(" and ", Filters);
            }
        }

        public string ColumnString
        {
            get
            {
                return string.Join(", ", Columns);
            }
        }
           

        #endregion
    }
}

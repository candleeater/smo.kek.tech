using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Api.v1
{
    public class ApiResult
    {
        public ApiResult() { }
        
        public ApiResult(PagingInfo info, IEnumerable<object> results, QueryInfo queryInfo)
        {
            Info = info;
            Info.CalculateStats(queryInfo);

            if (queryInfo.Randomize)
                Results = results.Shuffle(queryInfo.Seed).ToList();
            else
                Results = results.ToList();
        }

        public PagingInfo Info { get; set; }
        public List<object> Results { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Api.v1
{
    public class PagingInfo
    {
        public PagingInfo() { }

        public PagingInfo(int seed)
        {
            serializeSeed = true;
            Seed = seed;
        }

        private bool serializeSeed = false;
        private bool serializePageInfo = false;

        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int PageResults { get; set; }
        public int TotalPages { get; set; }
        public int TotalResults { get; set; }
        public int Seed { get; set; }


        public void SetSeed(int seed)
        {
            Seed = seed;
            serializeSeed = true;
        }

        public void CalculateStats(QueryInfo queryInfo)
        {
            if (PageSize > 0)
            {
                TotalPages = (TotalResults / PageSize) + (TotalPages % PageSize == 0 ? 0 : 1) + 1;

                if (PageNum > TotalPages)
                    PageResults = 0;
                else if (PageNum == TotalPages)
                    PageResults = (PageSize > TotalResults) ? TotalResults : TotalResults - ((PageNum - 1) * PageSize);
                else if (TotalResults < PageSize)
                    PageResults = TotalResults;
                else
                    PageResults = PageSize;

            }
            if (PageResults < 0)
                PageResults = 0;

            if (queryInfo != null)
            {
                if (queryInfo.Randomize)
                    SetSeed(queryInfo.Seed);

                if (queryInfo.IsPaged)
                    serializePageInfo = true;
            }
        }

        public bool ShouldSerializeSeed()
        {
            return serializeSeed;
        }

        public bool ShouldSerializePageNum()
        {
            return serializePageInfo;
        }

        public bool ShouldSerializePageSize()
        {
            return serializePageInfo;
        }

        public bool ShouldSerializePageResults()
        {
            return serializePageInfo;
        }

        public bool ShouldSerializeTotalPages()
        {
            return serializePageInfo;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Api.v1
{
    public static class StaticQueries
    {

        public const string OrderedMoonList = @"
select
    m.*,
	 case when m.orderBit & @orderBitMask != 0 then
        m.altorder
    else
        m.[order]  
    end as kingdomOrder
    {0}
from
    moondetail m
where
    {1}
order by
    case when orderBit & @orderBitMask != 0 then
        altorder
    else
        [order]  
    end,
    moonNumber
offset ((@pageNum - 1) * @pageSize) rows fetch next @pageSize rows only;
";

        public const string OrderedMoonListCount = @"
select
   count(*) as TotalResults,
   @pageNum as PageNum,
   @pageSize as PageSize
from
    moondetail m
where
    {0}
";

        public const string MoonTypeList = @"
select 
    m.* 
    {0}
from 
    moontype m
where 
    m.active = 1 and
    {1}
order by m.id
offset ((@pageNum - 1) * @pageSize) rows fetch next @pageSize rows only;
";

        public const string MoonTypeListCount = @"
select 
   count(*) as TotalResults,
   @pageNum as PageNum,
   @pageSize as PageSize
from 
    moontype m
where 
    m.active = 1 and
    {0}
";

        public const string KingdomList = @"
select 
    k.*,
	case when k.orderBit & @orderBitMask != 0 then
        k.altorder
    else
        k.[order]  
    end as kingdomOrder,
    coalesce(kw.warpKingdomId, k.warpKingdomId) as warpKingdomId
    {0}
from 
    kingdom k
    left join kingdomWarp kw on kw.kingdomId = k.id and kw.orderBitMask = @orderBitMask
where
    {1}
order by
    case when k.orderBit & @orderBitMask != 0 then
        k.altorder
    else
        k.[order]  
    end
offset ((@pageNum - 1) * @pageSize) rows fetch next @pageSize rows only;
";

        public const string KingdomListCount = @"
select 
   count(*) as TotalResults,
   @pageNum as PageNum,
   @pageSize as PageSize
from 
    kingdom k
where
    {0}
";

        public const string CaptureList = @"
select 
    c.*
    {0}
from 
    capture c
where
    {1}
order by c.captureNumber
offset ((@pageNum - 1) * @pageSize) rows fetch next @pageSize rows only;
";

        public const string CaptureListCount = @"
select 
   count(*) as TotalResults,
   @pageNum as PageNum,
   @pageSize as PageSize
from 
    capture c
where
    {0}
";

        public const string SongList = @"
select 
    s.*
    {0}
from
    song s
where
    {1}
order by s.songNumber
offset ((@pageNum - 1) * @pageSize) rows fetch next @pageSize rows only;
";

        public const string SongListCount = @"
select 
   count(*) as TotalResults,
   @pageNum as PageNum,
   @pageSize as PageSize
from 
    song s
where
    {0}
";

    }
}

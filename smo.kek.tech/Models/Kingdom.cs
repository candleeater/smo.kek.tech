using smo.kek.tech.Api.v1;
using smo.kek.tech.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{
    public class Kingdom : BaseModel
    {
        [QueryField(Name = "Id")]
        public int Id { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Name { get; set; }
        [QueryField(Name = "MoonCount")]
        public int MoonCount { get; set; }
        [QueryField(Name = "PurpleCoinCount")]
        public int PurpleCoinCount { get; set; }
        [QueryField(Name = "MoonColor")]
        public string MoonColor { get; set; }
        [QueryField(Name = "Order")]
        public int Order { get; set; }
        [QueryField(Name = "AltOrder")]
        public int AltOrder { get; set; }
        [QueryField(Name = "OrderBit")]
        public int OrderBit { get; set; }
        [QueryField(Name = "KingdomOrder")]
        public int KingdomOrder { get; set; }
        [QueryField(Name = "WarpKingdomId")]
        public int? WarpKingdomId { get; set; }
        [QueryField(Name = "MinimumMoons")]
        public int MinimumMoons { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Population { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Size { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Locals { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Currency { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Industry { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Temperature { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Locale { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Tagline { get; set; }
    }
}

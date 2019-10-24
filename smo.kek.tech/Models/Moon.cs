using Newtonsoft.Json;
using smo.kek.tech.Api.v1;
using smo.kek.tech.Models.Attributes;
using System.Collections.Generic;
using System.Linq;

namespace smo.kek.tech.Models
{
    [QueryClass(Alias = "m", SqlKey = "OrderedMoonList", SqlCountKey = "OrderedMoonListCount")]
    public class Moon : BaseModel
    {
        [QueryField(Name = "Id")]
        public int Id { get; set; }
        [QueryField(Name = "KingdomId")]
        public int KingdomId { get; set; }
        [QueryField(Name = "MoonNumber")]
        public int MoonNumber { get; set; }
        [QueryField(Name = "Kingdom", IsSearchable = true)]
        public string Kingdom { get; set; }
        [QueryField(Name = "Name", IsSearchable  = true)]
        public string Name { get; set; }
        [QueryField(Name = "IsMoonRockMoon"), Tooltip(Description = "Moon rock moons require beating the game and breaking the moon rock in this kingdom before it accessible")]
        public bool IsMoonRockMoon { get; set; }
        [QueryField(Name = "IsPostGame"), Tooltip(Description = "Post game moons are not available until completing the escape sequence in Moon Kingdom")]
        public bool IsPostGame { get; set; }
        [QueryField(Name = "IsSubAreaMoon")]
        public bool IsSubAreaMoon { get; set; }
        [QueryField(Name = "IsMultiMoon"), Tooltip(Description = "Multi moons increase your moon count by 3, but only have one entry in the moon list")]
        public bool IsMultiMoon { get; set; }
        [QueryField(Name = "IsRequired"), Tooltip(Description = "This moon is required to progress the game and cannot be skipped")]
        public bool IsRequired { get; set; }
        [QueryField(Name = "RequiresRevisit"), Tooltip(Description = "This moon cannot be collected on the first visit to a kingdom")]
        public bool RequiresRevisit { get; set; }
        [QueryField(Name = "IsStoryMoon")]
        public bool IsStoryMoon { get; set; }
        [QueryField(Name = "Value")]
        public int Value { get; set; }
        [QueryField(Name = "MoonColor")]
        public string MoonColor { get; set; }
        [QueryField(Name = "HintArtKingdomId")]
        public int? HintArtKingdomId { get; set; }
        public float XPos { get; set; }
        public float YPos { get; set; }
        [QueryField(Name = "Quadrant")]
        public string Quadrant { get; set; }
        [JsonIgnore]
        public int Order { get; set; }
        [JsonIgnore]
        public int? AltOrder { get; set; }
        [QueryField(Name = "OrderBit")]
        public int? OrderBit { get; set; }
        [QueryField(Name = "KingdomOrder")]
        public int KingdomOrder { get; set; }
        [JsonConverter(typeof(RawJsonConverter)),
            QueryField(Name = "MoonTypes", IsSearchable = true, IsList = true)]
        public string MoonTypes { get; set; }
        [JsonConverter(typeof(RawJsonConverter)),
            QueryField(Name = "MoonPrerequisites", IsSearchable = true, IsList = true)]
        public string MoonPrerequisites { get; set; }

        public bool HasMoonType(string type)
        {
            if (string.IsNullOrEmpty(this.MoonTypes))
                return false;
            var types = JsonConvert.DeserializeObject<List<MoonType>>(this.MoonTypes);

            if (types.Any(t => t.Name.ToLower() == type.ToLower()))
                return true;
            return false;
        }

        [JsonIgnore]
        public List<int> MoonPrerequisiteList
        {
            get
            {
                if (string.IsNullOrEmpty(this.MoonPrerequisites))
                    return new List<int>();
                var prereqs = JsonConvert.DeserializeObject<List<MoonPrereq>>(this.MoonPrerequisites);
                return prereqs.Select(p => p.Id).ToList();
            }
        }

        private class MoonPrereq
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

    }
}

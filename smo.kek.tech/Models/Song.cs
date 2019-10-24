using smo.kek.tech.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{
    public class Song : BaseModel
    {
        [QueryField(Name = "Id")]
        public int Id { get; set; }
        [QueryField(Name = "SongNumber")]
        public int SongNumber { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Name { get; set; }
    }
}

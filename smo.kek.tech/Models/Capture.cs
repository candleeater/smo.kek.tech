using smo.kek.tech.Api.v1;
using smo.kek.tech.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{
    public class Capture : BaseModel
    {
        [QueryField(Name = "Id")]
        public int Id { get; set; }
        [QueryField(Name = "CaptureNumber")]
        public int CaptureNumber { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Name { get; set; }
        [QueryField(Name = "Controls")]
        public string Controls { get; set; }
        [QueryField(Name = "Footnote")]
        public string Footnote { get; set; }
    }
}

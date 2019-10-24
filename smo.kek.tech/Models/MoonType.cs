using Newtonsoft.Json;
using smo.kek.tech.Api.v1;
using smo.kek.tech.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{
    public class MoonType : BaseModel
    {
        [QueryField(Name = "Id")]
        public int Id { get; set; }
        [QueryField(Name = "Name", IsSearchable = true)]
        public string Name { get; set; }
        [QueryField(Name = "Description")]
        public string Description { get; set; }
    }
}

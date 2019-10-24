using smo.kek.tech.Api.v1;
using smo.kek.tech.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{
    public class BaseModel
    {

        internal List<QueryField> GetQueryFields()
        {
            var list = new List<QueryField>();
            foreach(PropertyInfo info in GetType().GetProperties())
            {
                List<Attribute> attrs = info.GetCustomAttributes(typeof(QueryFieldAttribute), true).ToList();
                if (attrs.Any())
                {
                    foreach(var attr in attrs)
                    {
                        QueryFieldAttribute queryAttr = (QueryFieldAttribute)attr;

                        list.Add(new QueryField
                        {
                            Name = queryAttr.Name.ToLower(),
                            IsSearchable = queryAttr.IsSearchable,
                            IsList = queryAttr.IsList
                        });
                    }
                }
            }
            return list;
        }
    }
}

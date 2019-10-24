using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using smo.kek.tech.Api.v1;
using System;
using System.Collections.Generic;
using System.Linq;

namespace smo.kek.tech.Controllers.v1
{
    public class BaseController : Controller
    {
        public BaseController() : base()
        {

        }

        public virtual Dictionary<string, StringValues> GetQueryStrings(IQueryCollection query)
        {
            return query.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);
        }

        public virtual void SetDefaultQueryParameters(QueryInfo queryInfo)
        {
            if (!queryInfo.Parameters.ContainsKey("@pageNum"))
                queryInfo.Parameters.Add("@pageNum", 1);
            if (!queryInfo.Parameters.ContainsKey("@pageSize"))
                queryInfo.Parameters.Add("@pageSize", 1000);
            if (!queryInfo.Parameters.ContainsKey("@orderBitMask"))
                queryInfo.Parameters.Add("@orderBitMask", 0);
        }


        public virtual QueryInfo GetQueryInfo(string tableAlias, List<QueryField> fields)
        {
            var query = Request.Query;
            var queryStrings = query.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);
            var queryInfo = new QueryInfo();

            SetDefaultQueryParameters(queryInfo);

            foreach (var qkey in queryStrings.Keys)
            {
                var key = qkey.ToLower();
                var value = queryStrings[qkey].First();
                var delimiter = '.';

                var isDotNotation = (key.Contains(delimiter) && key.Count(c => c == delimiter) == 1);
                var fieldKey = isDotNotation ? key.Split(delimiter).First() : key;

                var field = fields.SingleOrDefault(f => f.Name.ToLower() == fieldKey);

                if (field != null)
                {
                    if (isDotNotation)
                    {
                        if (field.IsSearchable)
                        {
                            var fieldName = key.Split(delimiter).First();
                            var operation = key.Split(delimiter).Last();

                            if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(operation))
                            {
                                var paramName = $"@{fieldName}{operation}";

                                if (field.IsList)
                                {
                                    var values = value.Split(',');
                                    List<string> keys = new List<string>();

                                    switch (operation)
                                    {
                                        case "contains":
                                            
                                            for(int i = 0; i < values.Length; i++)
                                            {
                                                keys.Add($"{paramName}{i}");
                                                queryInfo.Parameters.Add($"{paramName}{i}", values[i]);
                                            }
                                            queryInfo.Filters.Add($"exists (select 1 from openjson({tableAlias}.[{fieldName}]) with (name varchar(500)) where name in ({string.Join(", ", keys)}))");

                                            break;
                                    }

                                }
                                else
                                {
                                    switch (operation)
                                    {
                                        case "contains":
                                            queryInfo.Filters.Add($"{tableAlias}.[{fieldName}] like '%' + {paramName} + '%'");
                                            queryInfo.Parameters.Add(paramName, value);

                                            break;
                                        case "startswith":
                                            queryInfo.Filters.Add($"{tableAlias}.[{fieldName}] like {paramName} + '%'");
                                            queryInfo.Parameters.Add(paramName, value);
                                            break;
                                        case "endswith":
                                            queryInfo.Filters.Add($"{tableAlias}.[{fieldName}] like '%' + {paramName}");
                                            queryInfo.Parameters.Add(paramName, value);
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (field.IsList)
                        {
                            queryInfo.Parameters.Add($"@{key}", value);
                            queryInfo.Filters.Add($"exists (select 1 from openjson({tableAlias}.[{key}]) with (name varchar(500)) where name = @{key})");
                        }
                        else
                        {
                            queryInfo.Parameters.Add($"@{key}", value);
                            queryInfo.Filters.Add($"{tableAlias}.[{key}] = @{key}");
                        }

                    }
                }
                else if (key == "pagenum" || key == "pagesize" || key == "orderbitmask")
                {
                    if (key == "pagesize")
                        queryInfo.IsPaged = true;

                    if (int.TryParse(value, out int intVal) && intVal > -1)
                    {
                        queryInfo.Parameters[$"@{key}"] = intVal;
                    }
                }
                else if (key == "randomize" && (value.ToLower() == "true" || value == "1"))
                {
                    queryInfo.Randomize = true;
                    queryInfo.Seed = new Random().Next(int.MaxValue);
                }
                else if (key == "seed")
                {
                    if (int.TryParse(value, out int seed))
                    {
                        queryInfo.Seed = seed;
                    }
                }
            }
            return queryInfo;
        }

        private string getKey(QueryInfo queryInfo, string key)
        {
            if (!key.StartsWith("@"))
                key = '@' + key;
            while (queryInfo.Parameters.ContainsKey(key))
            {
                var rand = new Random();
                key += rand.Next(10);
            }

            return key;
        }
    }
}

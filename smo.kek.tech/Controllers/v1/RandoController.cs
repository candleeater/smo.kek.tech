using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using smo.kek.tech.Api.v1;
using smo.kek.tech.Models;

namespace smo.kek.tech.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1")]
    public class RandoController : BaseController
    {
        private readonly string ConnectionString;

        IConfiguration Configuration;
        public RandoController(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("default");
        }

        /// <summary>
        /// Any% rando
        /// </summary>
        /// <returns></returns>
        [HttpGet("rando")]
        public async Task<JsonResult> GetAnyRando()
        {
            var query = Request.Query;
            var routes = new Dictionary<string, KingdomRoute>();
            int seed = new Random().Next(int.MaxValue);
            var rand = new Random(seed);
            var seedString = seed.ToString();

            if (query.ContainsKey("seed"))
            {
                if (int.TryParse(query["seed"], out seed))
                {
                    rand = new Random(seed);
                    seedString = seed.ToString();
                }
                else
                {
                    seed = query["seed"].GetHashCode();
                    seedString = $"{query["seed"].ToString()} ({seed})";
                    rand = new Random(seed);
                }
            }

            using (var conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    var moons = (await conn.QueryAsync<Moon>(@"select * from moondetail 
                        where ispostgame = 0 
                        and ismoonrockmoon = 0 
                        and requiresrevisit = 0 
                        and (moonTypes is null 
                        or (moonTypes not like '%seed%' 
                            and moonTypes not like '%warp%' 
                            and moonTypes not like '%hint art%' 
                            and moonTypes not like '%tourist%'))")).ToList();

                    var kingdoms = (await conn.QueryAsync<Kingdom>("select * from kingdom where id between 2 and 15")).ToList();

                    //add a route container for each kingdom
                    kingdoms.ForEach(k =>
                    {
                        routes.Add(k.Name, new KingdomRoute()
                        {
                            Kingdom = k,
                            IsPostGame = false,
                            IsRevisit = false,
                            Visit = 0
                        });
                    });

                    //add all the required moons to their appropriate kingdom
                    moons.Where(m => m.IsRequired).ToList().ForEach(m =>
                    {
                        routes[m.Kingdom].Tasks.Add(new RouteTask()
                        {
                            Completed = false,
                            Moon = m,
                            Type = TaskType.Moon,
                            Description = m.Name
                        });
                        moons.Remove(m);
                    });

                    //go through kingdoms again and fill with moons
                    foreach (var key in routes.Keys)
                    {
                        while (routes[key].MoonCount < routes[key].Kingdom.MinimumMoons)
                        {
                            var moon = moons.Where(m => m.Kingdom == key).Shuffle(rand).First();

                            if (moon.MoonPrerequisiteList.Any())
                            {
                                var missing = moon.MoonPrerequisiteList.Where(p => !routes[key].Moons.Select(m => m.Id).Contains(p));
                                while (missing.Any())
                                {
                                    moon = moons.Where(m => missing.Contains(m.Id)).Shuffle(rand).First();
                                    missing = moon.MoonPrerequisiteList.Where(p => !routes[key].Moons.Select(m => m.Id).Contains(p));
                                }

                            }

                            if ((moon.Value + routes[key].MoonCount) > routes[key].Kingdom.MinimumMoons)
                                continue;

                            routes[key].Tasks.Add(new RouteTask()
                            {
                                Moon = moon,
                                Type = TaskType.Moon,
                                Description = moon.Name
                            });

                            moons.Remove(moon);
                        }

                        //shopping moon coin task
                        if (routes[key].Tasks?.Any(m => m.Type == TaskType.Moon && m.Moon.HasMoonType("shopping")) ?? false)
                        {
                            routes[key].Tasks.Add(new RouteTask()
                            {
                                Type = TaskType.Task,
                                Description = "Get 100 coins for the shop moon"
                            });
                        }

                        routes[key].Tasks = routes[key].Tasks.OrderBy(t => t.Moon?.Id ?? 0).ToList();

                        //validate prereqs
                        var missingPrereqs = "";
                        routes[key].Tasks.ForEach(t =>
                        {
                            if (t.Type == TaskType.Moon)
                            {
                                if (t.Moon.MoonPrerequisiteList.Any())
                                {
                                    t.Moon.MoonPrerequisiteList.ForEach(p =>
                                    {
                                        if (!routes[key].Moons.Any(m => m.Id == p))
                                        {
                                            missingPrereqs += $"Found missing prerequisite moon: ${p}\n";
                                        }
                                    });
                                }
                            }
                        });

                        if (!string.IsNullOrEmpty(missingPrereqs))
                        {
                            return Json(new { error = "an error occured", seed = seedString, msg = $"Validation failed found missing prerequisites:\n {missingPrereqs}", ex = "", stack = "" });
                        }

                    }

                    #region static non-moon tasks

                    routes["Cap"].Tasks.Add(new RouteTask()
                    {
                        Completed = false,
                        Type = TaskType.Task,
                        Description = "Defeat Topper!"
                    });

                    routes["Cloud"].Tasks.Add(new RouteTask()
                    {
                        Completed = false,
                        Type = TaskType.Task,
                        Description = "Defeat Bowser!"
                    });

                    routes["Moon"].Tasks.Add(new RouteTask()
                    {
                        Completed = false,
                        Type = TaskType.Task,
                        Description = "Complete the escape sequence!"
                    });

                    #endregion


                    return Json(new
                    {
                        seed = seedString,
                        routes = routes.Values.ToList()
                    });

                }
                catch (Exception ex)
                {
                    return Json(new { error = "an error occured", seed = seedString, msg = ex.Message, ex = ex.InnerException, stack = ex.StackTrace });
                }
            }
        }
    }
}
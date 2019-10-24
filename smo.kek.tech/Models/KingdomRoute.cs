using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{
    public class KingdomRoute
    {
        public Kingdom Kingdom { get; set; }
        public int Visit { get; set; } = 1;
        public bool IsRevisit { get; set; } = false;
        public bool IsPostGame { get; set; } = false;
        public List<RouteTask> Tasks { get; set; } = new List<RouteTask>();

        public int MoonCount
        {
            get
            {
                return Tasks.Sum(t => t.Type == TaskType.Moon ? t.Moon.Value : 0);
            }
        }

        public int CompletedMoons { get; set; } = 0;

        [JsonIgnore]
        public List<Moon> Moons
        {
            get
            {
                return Tasks.Where(t => t.Type == TaskType.Moon).Select(t => t.Moon).ToList();
            }
        }
    }

    public class RouteTask
    {
        public TaskType Type { get; set; }
        public Moon Moon { get; set; }
        public string Description { get; set; }
        public bool Completed { get; set; } = false;
        
    }

    public enum TaskType
    {
        Task,
        Moon
    }
}

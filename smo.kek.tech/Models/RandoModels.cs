using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace smo.kek.tech.Models
{

    public class RandoResponse
    {
        public RandoRequestModel Settings { get; set; }
        public List<KingdomRoute> Routes { get; set; }
    }


    public class RandoErrorResponse
    {
        public RandoErrorResponse() { }

        public RandoErrorResponse(RandoRequestModel request)
        {
            Request = request;
        }

        public RandoErrorResponse(RandoRequestModel request, Exception ex)
        {
            Request = request;
            Message = ex.Message;
            InnerException = ex.InnerException?.Message ?? "";
            StackTrace = ex.StackTrace;
        }

        public string Error { get; set; } = "An error occurred";
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public RandoRequestModel Request { get; set; }
    }

    public class RandoRequestModel
    {
        public string Seed { get; set; }
        public bool ForceWorldPeace { get; set; } = false;

        public MoonSelectionStrategy SelectionStrategy { get; set; } = MoonSelectionStrategy.ResolvePrerequisites;

        #region advanced strats

        public bool AllowIPClip { get; set; } = false;
        public bool AllowSnowDram { get; set; } = false;
        public bool AllowSnowClip { get; set; } = false;

        #endregion

    }

    public enum MoonSelectionStrategy
    {
        ResolvePrerequisites = 1,
        DeferPrerequisites = 2
    }
}

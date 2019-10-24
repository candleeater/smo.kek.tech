using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;


namespace smo.kek.tech.Controllers.v1
{
    [Route("api/v1")]
    public class HomeController : Controller
    {

        [Route("info")]
        public ViewResult Info()
        {
            try
            {
                return View();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("index")]
        public ViewResult Index()
        {
            return View();
        }

    }
}
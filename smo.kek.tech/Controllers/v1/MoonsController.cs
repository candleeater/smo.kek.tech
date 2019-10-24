using Microsoft.AspNetCore.Mvc;
using Dapper;
using smo.kek.tech.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using smo.kek.tech.Api.v1;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace smo.kek.tech.Controllers.v1
{
    [Produces("application/json")]
    [Route("api/v1")]
    public class MoonsController : BaseController
    {
        private readonly string ConnectionString;

        IConfiguration Configuration;
        public MoonsController(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("default");
        }

        [HttpGet("moons")]
        public async Task<JsonResult> GetMoonList()
        {
            return await GetResults<Moon>("m", StaticQueries.OrderedMoonList, StaticQueries.OrderedMoonListCount);
        }

        [HttpGet("moons/{id:int}")]
        public async Task<JsonResult> GetMoon(int id)
        {
            return await GetById<Moon>("moondetail", id);
        }

        [HttpGet("moontypes")]
        public async Task<JsonResult> GetMoonTypes()
        {
            return await GetResults<MoonType>("m", StaticQueries.MoonTypeList, StaticQueries.MoonTypeListCount);
        }

        [HttpGet("kingdoms")]
        public async Task<JsonResult> GetKingdoms()
        {
            return await GetResults<Kingdom>("k", StaticQueries.KingdomList, StaticQueries.KingdomListCount);
        }

        [HttpGet("kingdoms/{id:int}")]
        public async Task<JsonResult> GetKingdom(int id)
        {
            return await GetById<Kingdom>("kingdom", id);
        }

        [HttpGet("captures")]
        public async Task<JsonResult> GetCaptures()
        {
            return await GetResults<Capture>("c", StaticQueries.CaptureList, StaticQueries.CaptureListCount);
        }

        [HttpGet("songs")]
        public async Task<JsonResult> GetSongs()
        {
            return await GetResults<Song>("s", StaticQueries.SongList, StaticQueries.SongListCount);
        }

        private async Task<JsonResult> GetResults<T>(string alias, string sql, string countSql) where T : BaseModel
        {
            var objT = Activator.CreateInstance(typeof(T));
            var queryFields = ((BaseModel)objT).GetQueryFields();

            var queryInfo = GetQueryInfo(alias, queryFields);

            sql = string.Format(sql, queryInfo.ColumnString, queryInfo.FilterString);
            countSql = string.Format(countSql, queryInfo.FilterString);

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    await conn.OpenAsync();

#if DEBUG
                    Console.WriteLine("count sql: " + countSql);
                    Console.WriteLine("sql: " + sql);
#endif

                    var info = (await conn.QueryAsync<PagingInfo>(countSql, queryInfo.Parameters)).First();
                    var results = (await conn.QueryAsync<T>(sql, queryInfo.Parameters)).Cast<object>();


                    var result = new ApiResult(info, results, queryInfo);
                    var jsonResult = Json(result);

                    return jsonResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<JsonResult> GetById<T>(string table, int id, string idColName = "id")
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var obj = (await conn.QueryAsync<T>($"select * from {table} a where a.{idColName} = @id", new { id })).FirstOrDefault();
                return Json(obj);
            }
        }
    }
}
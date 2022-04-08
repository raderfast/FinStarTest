using System.Linq.Expressions;
using System.Net;
using DBWorker;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using Newtonsoft.Json;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.Expressions.Internal;

namespace FinStarTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestServiceController : Controller
    {
        private readonly ILogger<TestServiceController> _logger;

        private readonly IConfiguration _configuration;

        public TestServiceController(ILogger<TestServiceController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// ��������� ������ ������ ����� �������� � �������
        /// </summary>
        /// <param name="values">������ </param>
        /// <returns></returns>
        private async Task<List<ValueSet>> ProcessBody(List<ValueSet> values)
        {
            values = values
                .OrderBy(b => b.Code)
                .ToList();

            foreach (var body in values)
                await Task.Run(() => body.OrderNumber = values.IndexOf(body) + 1);

            return values;
        }

        /// <summary>
        /// ���������� � ���������� ������� ����������� ������ �������� � ��
        /// ���������� ������� �������� ����������� ������
        /// </summary>
        /// <param name="values">������ ��������</param>
        /// <returns>���������� ���������� �������</returns>
        [HttpPost(Name = "Values")]
        [Route("PostValues")]
        public async Task<IActionResult> PostValues([FromBody]List<ValueSet> values)
        {
            try
            {
                Context context = new(_configuration.GetConnectionString("FinStarTest"));

                await Task.Run(() => context.ValueSets.RemoveRange(context.ValueSets));

                var bodies = await ProcessBody(values);

                await context.ValueSets.AddRangeAsync(bodies);
                await context.SaveChangesAsync();

                return Json(new List<Parameter>()
                {
                    new ()
                    {
                        Name = "ResultMessage",
                        Value = "������ ������� ���������."
                    },
                    new ()
                    {
                        Name = "IsError",
                        Value = "false"
                    }
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new List<Parameter>()
                {
                    new Parameter()
                    {
                        Name = "ResultMessage",
                        Value = e.Message
                    },
                    new Parameter()
                    {
                        Name = "IsError",
                        Value = "true"
                    }
                });
            }
        }

        /// <summary>
        /// ��������� ������ ��������� �� ��
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Values")]
        [Route("GetValues")]
        public async Task<IActionResult> GetValues([FromHeader]string? filterData, [FromHeader]int skip, [FromHeader]int take)
        {
            try
            {
                Context context = new(_configuration.GetConnectionString("FinStarTest"));

                List<ValueSet> bodies = new List<ValueSet>();

                if (filterData == null)
                {
                    bodies = await Task.Run(() => context.ValueSets
                        .Skip(skip).Take(take)
                        .ToList());
                }
                else
                {
                    bodies = await Task.Run(() => context.ValueSets
                        .Where(vs => EF.Functions.Like(vs.Value ?? String.Empty, filterData + "%"))
                        .Skip(skip).Take(take)
                        .ToList());
                }

                return Json(bodies);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new List<Parameter>()
                {
                    new ()
                    {
                        Name = "ResultMessage",
                        Value = e.Message
                    },
                    new ()
                    {
                        Name = "IsError",
                        Value = "true"
                    }
                });
            }
        }

        /// <summary>
        /// ��������� ������ ��������� �� ��
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "ValuesCount")]
        [Route("GetValuesCount")]
        public async Task<IActionResult> GetValuesCountAsync()
        {
            try
            {
                Context context = new(_configuration.GetConnectionString("FinStarTest"));

                var ValuesCount = await context.ValueSets.CountAsync();

                var parameter = new List<Parameter>()
                {
                    new Parameter()
                    {
                        Name = "ValuesCount",
                        Value = ValuesCount.ToString()
                    },
                    new ()
                    {
                        Name = "IsError",
                        Value = "false"
                    }
                };

                return Json(parameter);
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return Json(new List<Parameter>()
                {
                    new ()
                    {
                        Name = "ResultMessage",
                        Value = e.Message
                    },
                    new ()
                    {
                        Name = "IsError",
                        Value = "true"
                    }
                });
            }
        }
    }
}
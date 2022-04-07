using DBWorker;
using DBWorker.Models;
using Microsoft.AspNetCore.Mvc;

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
        private async Task<List<Value>> ProcessBody(List<Value> values)
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
        public async Task<IActionResult> Post([FromBody]List<Value> values)
        {
            try
            {
                Context context = new(_configuration.GetConnectionString("FinStarTest"));

                await Task.Run(() => context.Bodies.RemoveRange(context.Bodies));

                var bodies = await ProcessBody(values);

                await context.Bodies.AddRangeAsync(bodies);
                await context.SaveChangesAsync();

                return Json(new List<Parameter>()
                {
                    new ()
                    {
                        Name = "ResultMessage",
                        Value = "������ ������� ���������."
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
                        Value = "������!"
                    },
                    new Parameter()
                    {
                        Name = "IsError",
                        Value = e.Message
                    }
                });
            }
        }

        /// <summary>
        /// ��������� ������ ��������� �� ��
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Values")]
        public async Task<IActionResult> Get()
        {
            try
            {
                Context context = new(_configuration.GetConnectionString("FinStarTest"));

                var bodies = await Task.Run(() => context.Bodies.ToList());

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
                        Value = "������!"
                    },
                    new ()
                    {
                        Name = "IsError",
                        Value = e.Message
                    }
                });
            }
        }
    }
}
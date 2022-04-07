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
        /// Обработка набора данных перед вставкой в таблицу
        /// </summary>
        /// <param name="values">Список </param>
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
        /// Сортировка и сохранение массива содержащего список объектов в БД
        /// Присвоение каждому элементу порядкового номера
        /// </summary>
        /// <param name="values">Список объектов</param>
        /// <returns>Резулльтат выполнения запроса</returns>
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
                        Value = "Данные успешно обновлены."
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
                        Value = "Ошибка!"
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
        /// Получение списка элементов из БД
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
                        Value = "Ошибка!"
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
using DBWorker.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinStarTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestRestService : ControllerBase
    {
        private static List<Body> _bodyList = new();

        private readonly ILogger<TestRestService> _logger;

        public TestRestService(ILogger<TestRestService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Сортировка и сохранение массива содержащего список объектов в БД
        /// Присвоение каждому элементу порядкового номера
        /// </summary>
        /// <param name="bodyList">Список объектов</param>
        /// <returns>Резулльтат выполнения запроса</returns>
        [HttpPost(Name = "Values")]
        public IEnumerable<Body> Post([FromBody]List<Body> bodyList)
        {
            _bodyList = bodyList;
            return _bodyList.OrderBy(b => b.Code)
                .ToList();
        }

        /// <summary>
        /// Получение списка элементов из БД
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Values")]
        public IEnumerable<Body> Get()
        {
            return _bodyList;
        }
    }
}
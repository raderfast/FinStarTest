using DBWorker;
using DBWorker.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinStarTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestRestService : Controller
    {
        private readonly ILogger<TestRestService> _logger;

        private readonly IConfiguration _configuration;

        public TestRestService(ILogger<TestRestService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// ���������� � ���������� ������� ����������� ������ �������� � ��
        /// ���������� ������� �������� ����������� ������
        /// </summary>
        /// <param name="bodyList">������ ��������</param>
        /// <returns>���������� ���������� �������</returns>
        [HttpPost(Name = "Values")]
        public async Task<IActionResult> Post([FromBody]List<Body> bodyList)
        {
            Context context = new(_configuration.GetConnectionString("FinStarTest"));

            await Task.Run(() => context.Bodies.RemoveRange(context.Bodies));

            var bodies = bodyList
                .OrderBy(b => b.Code)
                .ToList();
            
            foreach (var body in bodies) body.OrderNumber = bodies.IndexOf(body) + 1;

            await context.Bodies.AddRangeAsync(bodies);
            await context.SaveChangesAsync();

            return Json(bodies);
        }

        /// <summary>
        /// ��������� ������ ��������� �� ��
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Values")]
        public async Task<IActionResult> Get()
        {
            Context context = new(_configuration.GetConnectionString("FinStarTest"));

            var bodies = await Task.Run(() => context.Bodies.ToList());

            return Json(bodies);
        }
    }
}
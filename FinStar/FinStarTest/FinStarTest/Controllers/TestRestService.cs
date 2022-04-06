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
        /// ���������� � ���������� ������� ����������� ������ �������� � ��
        /// ���������� ������� �������� ����������� ������
        /// </summary>
        /// <param name="bodyList">������ ��������</param>
        /// <returns>���������� ���������� �������</returns>
        [HttpPost(Name = "Values")]
        public IEnumerable<Body> Post([FromBody]List<Body> bodyList)
        {
            _bodyList = bodyList;
            return _bodyList.OrderBy(b => b.Code)
                .ToList();
        }

        /// <summary>
        /// ��������� ������ ��������� �� ��
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "Values")]
        public IEnumerable<Body> Get()
        {
            return _bodyList;
        }
    }
}
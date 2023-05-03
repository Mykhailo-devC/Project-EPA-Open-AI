using Epa.Engine.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EPA_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly EpaDbContext _dbContext;

        public WeatherForecastController(EpaDbContext epaDb, ILogger<WeatherForecastController> logger)
        {
            _dbContext = epaDb;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IEnumerable<TestModel>> Get()
        {

            return await _dbContext.testModels.ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var obj = new TestModel()
            {
                Name = "Test",
            };

            await _dbContext.AddAsync(obj);
            await _dbContext.SaveChangesAsync();

            return Ok(obj);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {
            var obj = await _dbContext.testModels.FindAsync(id);

            _dbContext.testModels.Remove(obj);
            await _dbContext.SaveChangesAsync();

            return Ok(obj);
        }
    }
}
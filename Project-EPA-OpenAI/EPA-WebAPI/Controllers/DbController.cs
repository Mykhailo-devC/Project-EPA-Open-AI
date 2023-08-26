using EPA.Engine.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EPA_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DbController : ControllerBase
    {
        private readonly EpaDbContext _dbContext;
        public DbController(EpaDbContext context)
        {
            _dbContext = context;
        }
        [HttpGet]
        public async Task<IActionResult> ReloadDataBase()
        {
            await _dbContext.Database.EnsureDeletedAsync();
            await _dbContext.Database.EnsureCreatedAsync();
            await _dbContext.SaveChangesAsync();

            return Ok("Database - reloaded");
        }
    }
}

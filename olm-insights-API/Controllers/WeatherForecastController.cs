using Microsoft.AspNetCore.Mvc;

namespace olm_insights_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OlmugController : ControllerBase
    {
        private readonly ILogger<OlmugController> _logger;
        private readonly ApplicationDbContext dbContext;

        public OlmugController(ILogger<OlmugController> logger, ApplicationDbContext dbContext)
        {
            _logger = logger;
            this.dbContext = dbContext;
        }

        [HttpGet(Name = "Ping")]
        public IActionResult Ping()
        {
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Blog> Create(Blog blog)
        {
            if (blog != null && !string.IsNullOrEmpty(blog.Title))
            {
                _logger.LogInformation("adding blog with title: " + blog.Title);

                this.dbContext.Blogs.Add(blog);

                var httpClient 
            }
            else
            {
                _logger.LogInformation("not adding");
            }


            return CreatedAtAction(nameof(Create), new { id = Guid.NewGuid().ToString() }, blog);
        }
    }
}
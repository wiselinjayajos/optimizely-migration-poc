using CMSDemo.Migration.Models;
using CMSDemo.Migration.Services;
using Microsoft.AspNetCore.Mvc;

namespace CMSDemo.Migration.Controllers
{
    [Route("api/content")]
    [ApiController]
    public class ContentMigrationController : ControllerBase
    {
        private readonly IContentMigrationService _migrationService;

        public ContentMigrationController(IContentMigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        [HttpGet("Status")]
        public IActionResult GetServiceStatus()
        {
            return Ok("Content Migration Service is running successfully.");
        }

        /// <summary>
        /// Migrate content to Optimizely CMS (bulk insert/update).
        /// </summary>
        [HttpPost("migrate")]
        public async Task<IActionResult> MigrateContent([FromBody] List<ContentItem> contentItems)
        {
            if (contentItems == null || contentItems.Count == 0)
            {
                return BadRequest("No content items provided.");
            }

            await _migrationService.MigrateContentAsync(contentItems);
            return Ok("Content migration completed successfully");
        }

        /// <summary>
        /// Delete content by GUID.
        /// </summary>
        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteContent([FromQuery] int contentId)
        {
            bool deleted = await _migrationService.DeleteContentAsync(contentId);
            if (deleted)
            {
                return Ok($"Content with Id {contentId} deleted successfully.");
            }

            return NotFound($"Content with Id {contentId} not found.");
        }
    }
}

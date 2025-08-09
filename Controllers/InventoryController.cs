using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;

namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly LogiTrackContext _context;

        public InventoryController(LogiTrackContext context)
        {
            _context = context;
        }

        // GET: api/inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItem>>> GetInventory()
        {
            var items = await Task.FromResult(_context.InventoryItems.ToList());
            return Ok(items);
        }

        // POST: api/inventory
        [HttpPost]
        public async Task<ActionResult<InventoryItem>> CreateItem([FromBody] InventoryItem item)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.InventoryItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventory), new { id = item.ItemId }, item);
        }
    }
}

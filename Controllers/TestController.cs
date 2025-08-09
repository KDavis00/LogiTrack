using Microsoft.AspNetCore.Mvc;
using LogiTrack.Models;

namespace LogiTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly LogiTrackContext _context;

        public TestController(LogiTrackContext context)
        {
            _context = context;
        }

        [HttpGet("summary")]
        public ActionResult<string> GetOrderSummary()
        {
            var order = new Order
            {
                OrderId = 1001,
                CustomerName = "Samir"
            };

            var palletJack = _context.InventoryItems.FirstOrDefault(i => i.Name == "Pallet Jack");
            if (palletJack != null)
            {
                order.AddItem(palletJack);
                order.AddItem(new InventoryItem { ItemId = 2, Name = "Forklift", Quantity = 1, Location = "Warehouse B" });
                order.RemoveItem(2);
            }

            return Ok(order.GetOrderSummary());
        }
    }
}

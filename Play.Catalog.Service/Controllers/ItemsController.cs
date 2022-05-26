using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.Dtos;

namespace Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> _items = new List<ItemDto>
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
         };

        // GET: api/items
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return _items;
        }

        // GET api/items/5
        [HttpGet("{id}")]
        public ItemDto Get(Guid id)
        {
            var result = _items.SingleOrDefault(q => q.Id == id);
            return result;
        }

        // POST api/items
        [HttpPost]
        public ActionResult<ItemDto> Post([FromBody] CreateItemDto body)
        {
            var created = new ItemDto(Guid.NewGuid(), body.Name, body.Description, body.Price, DateTimeOffset.UtcNow);
            _items.Add(created);
            return CreatedAtAction(nameof(Get), new { Id = created.Id }, created);
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] UpdateItemDto body)
        {
            var existing = _items.SingleOrDefault(q => q.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            var updated = existing with
            {
                Name = body.Name,
                Description = body.Description,
                Price = body.Price,
            };

            var existingIndex = _items.FindIndex(q => q.Id == id);
            _items[existingIndex] = updated;

            return NoContent();
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var existingIndex = _items.FindIndex(q => q.Id == id);

            if (existingIndex == -1)
            {
                return NotFound();
            }

            _items.RemoveAt(existingIndex);
            return NoContent();
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using Mozart.Play.Catalog.Service.Dtos;

namespace Mozart.Play.Catalog.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private static readonly List<ItemDto> _items = new()
        {
            new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Antidote", "Cures Poison", 7, DateTimeOffset.UtcNow),
            new ItemDto(Guid.NewGuid(), "Sword", "Deals damage", 20, DateTimeOffset.UtcNow)
        };

        // GET: api/<ItemsController>
        [HttpGet]
        public IEnumerable<ItemDto> Get()
        {
            return _items;
        }

        // GET api/<ItemsController>/5
        [HttpGet("{id}")]
        public ActionResult<ItemDto> Get(Guid id)
        {
            var result = _items.SingleOrDefault(q => q.Id == id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST api/<ItemsController>
        [HttpPost]
        public ActionResult<ItemDto> Post(CreateItemDto value)
        {
            var create = new ItemDto(Guid.NewGuid(), value.Name, value.Description, value.Price, DateTimeOffset.UtcNow);
            _items.Add(create);

            return CreatedAtAction(nameof(Get), new { id = create.Id }, create);
        }

        // PUT api/<ItemsController>/5
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, UpdateItemDto value)
        {
            var existing = _items.SingleOrDefault(q => q.Id == id);

            if (existing == null)
            {
                return NotFound();
            }

            var updated = existing with
            {
                Name = value.Name,
                Description = value.Description,
                Price = value.Price
            };

            var existingIndex = _items.FindIndex(q => q.Id == id);
            _items[existingIndex] = updated;

            return NoContent();
        }

        // DELETE api/<ItemsController>/5
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

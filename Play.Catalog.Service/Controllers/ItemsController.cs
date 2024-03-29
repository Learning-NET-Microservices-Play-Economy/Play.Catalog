﻿using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Mozart.Play.Catalog.Contracts;
using Mozart.Play.Catalog.Service.Dtos;
using Mozart.Play.Catalog.Service.Entities;
using Mozart.Play.Common;

namespace Mozart.Play.Catalog.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        // private static readonly List<ItemDto> _items = new List<ItemDto>
        // {
        //     new ItemDto(Guid.NewGuid(), "Potion", "Restores a small amount of HP", 5, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Antidote", "Cures poison", 7, DateTimeOffset.UtcNow),
        //     new ItemDto(Guid.NewGuid(), "Bronze Sword", "Deals a small amount of damage", 20, DateTimeOffset.UtcNow)
        //  };

        private readonly IRepository<Item> _itemsRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ItemsController(IRepository<Item> itemsRepository, IPublishEndpoint publishEndpoint)
        {
            _itemsRepository = itemsRepository;
            _publishEndpoint = publishEndpoint;
        }

        // GET: api/items
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDto>>> GetAsync()
        {
            var result = (await _itemsRepository.GetManyAsync())
                        .Select(q => q.AsDto());
            return Ok(result);
        }

        // GET api/items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetAsync(Guid id)
        {
            var result = await _itemsRepository.GetAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result.AsDto();
        }

        // POST api/items
        [HttpPost]
        public async Task<ActionResult<ItemDto>> PostAsync([FromBody] CreateItemDto body)
        {
            var entity = new Item
            {
                Name = body.Name,
                Description = body.Description,
                Price = body.Price,
                CreatedDate = DateTimeOffset.UtcNow
            };

            await _itemsRepository.CreateAsync(entity);

            await _publishEndpoint.Publish(new CatalogItemCreated(entity.Id, entity.Name, entity.Description));

            return CreatedAtAction(nameof(GetAsync), new { Id = entity.Id }, entity);
        }

        // PUT api/items/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAsync(Guid id, [FromBody] UpdateItemDto body)
        {
            var existing = await _itemsRepository.GetAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = body.Name;
            existing.Description = body.Description;
            existing.Price = body.Price;

            await _itemsRepository.UpdateAsync(existing);

            await _publishEndpoint.Publish(new CatalogItemUpdated(existing.Id, existing.Name, existing.Description));

            return NoContent();
        }

        // DELETE api/items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var existing = await _itemsRepository.GetAsync(id);

            if (existing == null)
            {
                return NotFound();
            }

            await _itemsRepository.DeleteAsync(id);

            await _publishEndpoint.Publish(new CatalogItemDeleted(existing.Id));

            return NoContent();
        }
    }
}


using Mozart.Play.Catalog.Service.Dtos;
using Mozart.Play.Catalog.Service.Entities;

namespace Mozart.Play.Catalog.Service
{
    public static class Extensions
    {
        public static ItemDto AsDto(this Item item)
        {
            return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedDate);
        }
    }
}


using MongoDB.Driver;
using Mozart.Play.Catalog.Service.Entities;

namespace Mozart.Play.Catalog.Service.Repositories
{
    public class ItemsRepository
    {
        private const string _collectionName = "items";
        private readonly IMongoCollection<Item> _dbCollection;
        private readonly FilterDefinitionBuilder<Item> _filterBuilder = Builders<Item>.Filter;

        public ItemsRepository()
        {
            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var database = mongoClient.GetDatabase("Catalog");
            _dbCollection = database.GetCollection<Item>(_collectionName);
        }

        public async Task<IReadOnlyCollection<Item>> GetAsync()
        {
            return await _dbCollection.Find(_filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(q => q.Id, id);
            return await (await _dbCollection.FindAsync(filter)).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _dbCollection.InsertOneAsync(entity);
        }

        public async Task UpdateAsync(Item entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var filter = _filterBuilder.Eq(q => q.Id, entity.Id);
            await _dbCollection.ReplaceOneAsync(filter, entity);
        }

        public async Task DeleteAsync(Guid id)
        {
            var filter = _filterBuilder.Eq(q => q.Id, id);
            await _dbCollection.DeleteOneAsync(filter);
        }
    }
}

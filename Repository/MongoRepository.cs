using MongoDB.Bson;
using MongoDB.Driver;
using MongoDbClient.Entities;
using MongoDbClient.Settings;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MongoDbClient.Repository
{
    public class MongoRepository<T> where T : MongoEntity
    {
        private readonly IMongoCollection<T> _mongoCollection;

        private MongoClient _client;

        private IMongoDatabase _database;

        public MongoRepository(IMongoSettings databaseSettings)
        {
            _client = new MongoClient(databaseSettings.ConnectionString);
            _database = _client.GetDatabase(databaseSettings.DatabaseName);
            _mongoCollection = _database.GetCollection<T>(databaseSettings.ContainerName);
        }

        /// <summary>
        /// Get a specific element from mongo collection.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throws when id is empty or null</exception>
        /// <exception cref="ArgumentException">throws when id is not a valid GUID</exception>
        public T Get(string id)
        {
            Validate(id);

            return _mongoCollection.Find(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Create an element in mongo collection
        /// </summary>
        /// <param name="element"></param>
        /// <exception cref="ArgumentNullException">throws when element is null</exception>
        public void Create(T element)
        {
            Validate(element, false);

            _mongoCollection.InsertOne(element);
        }

        /// <summary>
        /// Update specific element of mongo collection
        /// </summary>
        /// <param name="element"></param>
        /// <exception cref="ArgumentNullException">throws when element or element.Id is null or empty</exception>
        /// <exception cref="ArgumentException">throws when element.Id is not a valid GUID</exception>
        /// <exception cref="MongoClientException">throws when IsAcknowledged is false</exception>
        public void Update(T element)
        {
            Validate(element);

            _mongoCollection.ReplaceOne(x => x.Id == element.Id, element);
        }

        /// <summary>
        /// Delete an element from mongo collection
        /// </summary>
        /// <param name="id"></param>
        /// <exception cref="ArgumentNullException">throws when id is empty or null</exception>
        /// <exception cref="ArgumentException">throws when id is not a valid GUID</exception>
        /// <exception cref="MongoClientException">throws when IsAcknowledged is false</exception>
        public void Remove(string id)
        {
            Validate(id);

            DeleteResult result = _mongoCollection.DeleteOne(x => x.Id == id);

            if (!result.IsAcknowledged)
                throw new MongoClientException($"Remove of element {id} is not completed.");
        }

        /// <summary>
        /// Returns all elements from query.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<T> Query() => _mongoCollection.Find(element => true).ToList();

        /// <summary>
        /// Finds an returns elements with specified expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(Expression<Func<T, bool>> expression) => _mongoCollection.Find(expression).ToList();

        /// <summary>
        /// Create an element in your mongo collection
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throws when element is null</exception>
        public async Task<T> CreateAsync(T element)
        {
            Validate(element, false);

            await _mongoCollection.InsertOneAsync(element);

            return element;
        }

        /// <summary>
        /// Update an element in your mongo collection
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throws when element or element.Id is null or empty</exception>
        /// <exception cref="ArgumentException">throws when element.Id is not a valid GUID</exception>
        /// <exception cref="MongoClientException">throws when IsAcknowledged is false</exception>
        public async Task<T> UpdateAsync(T element)
        {
            Validate(element);

            ReplaceOneResult result = await _mongoCollection.ReplaceOneAsync(x => x.Id == element.Id, element);

            if (!result.IsAcknowledged)
                throw new MongoClientException($"UpdateAsync of element {element.Id} is not completed.");

            return element;
        }

        /// <summary>
        /// Remove an element from mongo collection
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">throws when element or id is null or empty</exception>
        /// <exception cref="ArgumentException">throws when id is not a valid GUID</exception>
        /// <exception cref="MongoClientException">throws when IsAcknowledged is false</exception>
        public async Task RemoveAsync(string id)
        {
            Validate(id);

            DeleteResult result = await _mongoCollection.DeleteOneAsync(x => x.Id == id);

            if (!result.IsAcknowledged)
                throw new MongoClientException($"RemoveAsync of element {id} is not completed.");
        }

        private void Validate(T element, bool update = true)
        {
            if (element == null)
                throw new ArgumentNullException($"{nameof(element)} is null");

            if (update)
            {
                if (string.IsNullOrEmpty(element.Id))
                    throw new ArgumentNullException($"{nameof(element.Id)} is null or empty");

                if (ObjectId.Parse(element.Id) == ObjectId.Empty)
                    throw new ArgumentException($"{nameof(element.Id)} is not a valid ObjectId");
            }
        }

        private void Validate(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException($"{nameof(id)} is null or empty");

            if (ObjectId.Parse(id) == ObjectId.Empty)
                throw new ArgumentException($"{nameof(id)} is not a valid GUID");
        }
    }
}

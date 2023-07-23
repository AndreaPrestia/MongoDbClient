using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbClient.Entities
{
    /// <summary>
    /// Base entity
    /// </summary>
    public class MongoEntity
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}

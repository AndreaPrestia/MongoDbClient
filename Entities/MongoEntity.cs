using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDbClient.Entities
{
    public class MongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}

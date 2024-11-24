using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Pcf.Administration.Core.Domain;

public class MongoBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
}
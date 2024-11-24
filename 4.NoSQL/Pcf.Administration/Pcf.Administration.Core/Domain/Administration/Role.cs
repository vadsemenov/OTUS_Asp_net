using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Pcf.Administration.Core.Domain.Administration
{
    public class Role
        : MongoBaseEntity
    {
        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }
    }
}
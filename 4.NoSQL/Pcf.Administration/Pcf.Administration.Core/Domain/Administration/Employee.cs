using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace Pcf.Administration.Core.Domain.Administration
{
    public class Employee
        : MongoBaseEntity
    {
        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        [BsonElement("Email")]
        public string Email { get; set; }

        // public Guid RoleId { get; set; }

        [BsonElement("Role")]
        public virtual Role Role { get; set; }

        [BsonElement("AppliedPromocodesCount")]
        public int AppliedPromocodesCount { get; set; }
    }
}
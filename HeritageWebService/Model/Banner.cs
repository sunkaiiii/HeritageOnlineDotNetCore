using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebService.Model
{
    public class Banner
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public string Id { get; set; }

        [BsonElement("link")]
        [Required]
        public string Link { get; set; }

        [BsonElement("img")]
        [Required]
        public string Img { get; set; }
    }
}

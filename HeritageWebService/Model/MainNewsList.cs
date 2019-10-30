using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebService.Model
{
    public class MainNewsList
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]

        public string Id { get; set; }

        [BsonElement("link")]
        [Required]
        public string DetailPageUrl { get; set; }

        [BsonElement("text")]
        [Required]
        public string Title { get; set; }

        [BsonElement("date")]
        [Required]
        public string Date { get; set; }
    }
}

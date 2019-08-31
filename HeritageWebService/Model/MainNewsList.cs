using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebService.Model
{
    public class MainNewsList
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("url")]
        public string DetailPageUrl { get; set; }
        [BsonElement("text")]
        public string Title { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
    }
}

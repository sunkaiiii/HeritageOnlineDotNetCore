using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class NewsList
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("link")]
        public string Link { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("img")]
        public string Img { get; set; }
        [BsonElement("date")]
        public string Date { get; set; }
        [BsonElement("content")]
        public string Content{get;set;}
        [BsonElement("compressImg")]
        public string CompressImg { get; set; }
    }
}

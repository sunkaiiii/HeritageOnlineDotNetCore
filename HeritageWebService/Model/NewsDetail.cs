using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class NewsDetail
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("link")]
        public string Link { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("subtitle")]
        public List<string> Subtitle { get; set; }
        [BsonElement("content")]
        public List<Item> Content { get; set; }
        [BsonElement("author")]
        public string Author { get; set; }
        [BsonElement("relativeNews")]
        public List<BottomRelativeNews> RelativeNews { get; set; }

       public class Item
        {
            [BsonElement("type")]
            public string Type { get; set; }
            [BsonElement("content")]
            public string Content { get; set; }
            [BsonElement("compressImg")]
            public string CompressImg { get; set; }
        }

        public class BottomRelativeNews
        {
            [BsonElement("link")]
            public string Link { get; set; }
            [BsonElement("date")]
            public string Date { get; set; }
            [BsonElement("title")]
            public string Title { get; set; }
        }
    }
}

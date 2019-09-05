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
        public string link;
        public string title;
        public List<string> subtitle;
        public List<Item> content;
        public string author;
        public List<BottomRelativeNews> relativeNews;

       public class Item
        {
            public string type;
            public string content;
        }

        public class BottomRelativeNews
        {
            public string link;
            public string date;
            public string title;
        }
    }
}

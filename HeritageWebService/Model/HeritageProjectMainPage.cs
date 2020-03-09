using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageProjectMainPage
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set;}
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("content")]
        public string Content { get; set; }
        [BsonElement("numItem")]
        public List<NumberItem> NumItem { get; set; }

        public class NumberItem
        {
            [BsonElement("num")]
            public string Num { get; set; }
            [BsonElement("desc")]
            public string Desc { get; set; }
        }
    }
}

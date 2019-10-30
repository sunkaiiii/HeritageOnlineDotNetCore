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
        public string title;
        public string content;
        public List<NumberItem> numItem;

        public class NumberItem
        {
            public string num;
            public string desc;
        }
    }
}

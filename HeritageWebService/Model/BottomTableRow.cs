using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class BottomTableRow
    {
        [BsonElement("link")]
        public string Link { get; set; }
        [BsonElement("content")]
        public List<TableContent> Content { get; set; }
        public class TableContent
        {
            [BsonElement("key")]
            public string Key { get; set; }
            [BsonElement("value")]
            public string Value { get; set; }
        }
    }
}

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
        [BsonElement("mapTables")]
        public List<MapInformation> MapTables { get; set; } 

        public class NumberItem
        {
            [BsonElement("num")]
            public string Num { get; set; }
            [BsonElement("desc")]
            public string Desc { get; set; }
        }

        public class MapInformation
        {
            [BsonElement("desc")]
            public string Desc { get; set; }
            [BsonElement("content")]
            public MapInformationContent content { get; set; }
            public class MapInformationContent
            {
                [BsonElement("desc")]
                public List<string> Desc { get; set; }
                [BsonElement("table")]
                public List<MapTable> Table { get; set; }
                [BsonElement("total")]
                public string Total { get; set; }
                public class MapTable
                {
                    [BsonElement("num")]
                    public string Num { get; set; }
                    [BsonElement("desc")]
                    public string Desc { get; set; }
                    [BsonElement("searchLink")]
                    public string SearchLink { get; set; }
                }
            }
        }
    }
}

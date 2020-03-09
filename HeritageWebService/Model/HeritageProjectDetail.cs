using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageProjectDetail
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonElement("title")]
        public string Title { get; set; }
        [BsonElement("link")]
        public string Link { get; set; }
        [BsonElement("text")]
        public string Text { get; set; }
        [BsonElement("desc")]
        public List<string> Desc { get; set; }
        [BsonElement("inheritate")]
        public List<BottomTableRow> Inheritate { get; set; }
        [BsonElement("ralevant")]
        public List<BottomTableRow> Ralevant { get; set; }
    }
}

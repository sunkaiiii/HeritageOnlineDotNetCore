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

        public string title;
        public string link;
        public string text;
        public List<string> desc;
        public List<BottomTableRow> inheritate;
        public List<BottomTableRow> ralevant;

        public class BottomTableRow
        {
            public string link;
            public List<TableContent> content;
        }
        public class TableContent
        {
            public string key;
            public string value;
        }
    }
}

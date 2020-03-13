using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class PeoplePage
    {
        public class PeopleMainPage
        {
            [BsonId]
            [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
            public string Id { get; set; }

            [BsonElement("table")]
            public List<PeopleMainPageTableContent> Table { get; set; }

            public class PeopleMainPageTableContent
            {
                [BsonElement("img")]
                public string Img { get; set; }
                [BsonElement("desc")]
                public string Desc { get; set; }
                [BsonElement("link")]
                public string Link { get; set; }
            }
        }
    }
}

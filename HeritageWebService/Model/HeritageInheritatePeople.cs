using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageInheritatePeople
    {


        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        public string Id { get; set; }
        public string title;
        public string link;
        public List<string> desc;
        public string text;
        public List<BottomTableRow> inheritate;
    }
}

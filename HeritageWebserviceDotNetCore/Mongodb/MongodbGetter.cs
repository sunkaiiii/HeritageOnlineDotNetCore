using System;
using HeritageWebserviceDotNetCore.Mongodb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HeritageWebserviceReptileDotNetCore.Mongodb
{
    public static class MongodbGetter
    {
        public static BsonDocument GetHeritageProjectMainPageDesc()
        {
            var collection = MongodbMain.GetCollection(MongodbMain.HeritageProjectMainPage);
            var result = collection.Find<BsonDocument>(new BsonDocument());
            if(result.Count()==0)
            {
                return null;
            }
            return result.First<BsonDocument>();
        }
    }
}

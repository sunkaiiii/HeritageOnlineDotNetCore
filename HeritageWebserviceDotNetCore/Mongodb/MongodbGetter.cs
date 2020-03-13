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
            if (result == null || result.CountDocuments() == 0)
            {
                return null;
            }
            return result.First<BsonDocument>();
        }

        public static BsonDocument GetACollection(string collectionName)
        {
            var collection = MongodbMain.GetCollection(collectionName);
            var result = collection.Find<BsonDocument>(new BsonDocument());
            if (result == null || result.CountDocuments() == 0)
            {
                return null;
            }
            return result.First();
        }
    }
}

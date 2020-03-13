using System;
using HeritageWebserviceDotNetCore.Mongodb;
using MongoDB.Bson;
using MongoDB.Driver;

namespace HeritageWebserviceReptileDotNetCore.Mongodb
{
    public static class MongodbUpdater
    {
        public static void UpdateHeritageProjectMainContent(BsonDocument bson)
        {
            MongodbMain.GetCollection(MongodbMain.HeritageProjectMainPage).FindOneAndUpdate(new BsonDocument(), bson);
        }

        public static void UpdateACollection(BsonDocument newInfo, string collectionName) => UpdateACollection(newInfo, collectionName, new BsonDocument());
        public static void UpdateACollection(BsonDocument newInfo, string collectionName, BsonDocument condition)
        {
            MongodbMain.GetCollection(collectionName).FindOneAndUpdate(condition, newInfo);
        }
    }
}

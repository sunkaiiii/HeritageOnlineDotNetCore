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
    }
}

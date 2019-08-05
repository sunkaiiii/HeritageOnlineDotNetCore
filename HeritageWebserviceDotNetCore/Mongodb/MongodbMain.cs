using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public sealed class MongodbMain
    {
        private static readonly Lazy<MongodbMain> lazy = new Lazy<MongodbMain>(() => new MongodbMain());
        public static MongodbMain Instance { get { return lazy.Value; } }

        private readonly MongoClient client;
        private readonly IMongoDatabase database;
        private readonly IMongoCollection<BsonDocument> collection;
        private MongodbMain()
        {
            client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("heritage");
            collection = database.GetCollection<BsonDocument>("heritage"); 
        }
        ~MongodbMain()
        {
          
        }

        internal void SaveMainpageNewsList(IEnumerable<BsonDocument> nodes)
        {
            collection.InsertMany(nodes);
        }
    }
}

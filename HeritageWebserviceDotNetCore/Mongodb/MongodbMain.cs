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

        internal static readonly string NEWS_LIST = "news_list";
        private readonly MongoClient client;
        internal readonly IMongoDatabase database;
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

        public void SaveMainpageNewsList(IEnumerable<BsonDocument> nodes)
        {
            collection.InsertMany(nodes);
        }

        public void SaveNewsList(IEnumerable<BsonDocument> nodes)
        {
            var collection = database.GetCollection<BsonDocument>(NEWS_LIST);
            collection.InsertMany(nodes);
        }

        public bool DeleteNews(BsonDocument filter)
        {
            var collection = database.GetCollection<BsonDocument>(NEWS_LIST);
            var result = collection.FindOneAndDelete<BsonDocument>(filter);
            return result != null;
        }
    }
}

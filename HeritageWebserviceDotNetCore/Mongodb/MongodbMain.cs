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
        internal static readonly string NEWS_DETAIL = "news_detail";
        internal static readonly string FORUMS_LIST = "forums_list";
        internal static readonly string FORUMS_DETAIL = "forums_detail";
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

        public void SaveNewsDetail(BsonDocument bson)
        {
            var collection = database.GetCollection<BsonDocument>(NEWS_DETAIL);
            collection.InsertOne(bson);
        }

        public void SaveForumsList(IEnumerable<BsonDocument> bsons)
        {
            var collection = database.GetCollection<BsonDocument>(FORUMS_LIST);
            collection.InsertMany(bsons);
        }

        public void SaveForumsDetail(BsonDocument bson)
        {
            var collection = database.GetCollection<BsonDocument>(FORUMS_DETAIL);
            collection.InsertOne(bson);
        }
    }
}

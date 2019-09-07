using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    internal sealed class MongodbMain
    {
        private static readonly Lazy<MongodbMain> lazy = new Lazy<MongodbMain>(() => new MongodbMain());
        public static MongodbMain Instance { get { return lazy.Value; } }

        internal static readonly string MAIN_PAGE = "main_page";
        internal static readonly string NEWS_LIST = "news_list";
        internal static readonly string NEWS_DETAIL = "news_detail";
        internal static readonly string FORUMS_LIST = "forums_list";
        internal static readonly string FORUMS_DETAIL = "forums_detail";
        internal static readonly string SPECIFIC_TOPIC = "specific_topic";
        internal static readonly string SPECIFIC_TOPIC_DETAIL = "specific_topic_detail";
        internal static readonly string HERITAGE_PROJECT_MAIN_PAGE = "heritage_project_main_page";
        internal static readonly string HERITAGE_PROJECT = "heritage_project";

        internal Dictionary<string, MongoDB.Driver.IMongoCollection<BsonDocument>> COLLECTIONS;
        private readonly MongoClient client;
        internal readonly IMongoDatabase Database;
        private MongodbMain()
        {
            client = new MongoClient("mongodb://localhost:27017");
            Database = client.GetDatabase("heritage");
            List<string> collectionString = new List<string>
            {
                MAIN_PAGE,
                NEWS_LIST,
                NEWS_DETAIL,
                FORUMS_LIST,
                FORUMS_DETAIL,
                SPECIFIC_TOPIC,
                SPECIFIC_TOPIC_DETAIL,
                HERITAGE_PROJECT_MAIN_PAGE,
                HERITAGE_PROJECT
            };
            COLLECTIONS = new Dictionary<string, IMongoCollection<BsonDocument>>();
            foreach (var collectionName in collectionString)
            {
                COLLECTIONS[collectionName] = Database.GetCollection<BsonDocument>(collectionName);
            }
  
        }

        public static IMongoCollection<BsonDocument> GetCollection(string collectionName)
        {
            return Instance.COLLECTIONS[collectionName];
        }

        ~MongodbMain()
        {
          
        }
    }
}

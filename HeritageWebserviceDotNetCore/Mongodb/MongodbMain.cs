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
        private readonly MongoClient client;
        internal readonly IMongoDatabase Database;
        private MongodbMain()
        {
            client = new MongoClient("mongodb://localhost:27017");
            Database = client.GetDatabase("heritage");
  
        }
        ~MongodbMain()
        {
          
        }
    }
}

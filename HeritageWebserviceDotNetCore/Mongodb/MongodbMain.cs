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

        internal const string Banner = "banner";
        internal const string NewsList = "news_list";
        internal const string NewsDetail = "news_detail";
        internal const string ForumsList = "forums_list";
        internal const string ForumsDetail = "forums_detail";
        internal const string SpecificTopic = "specific_topic";
        internal const string SpecificTopicDetail = "specific_topic_detail";
        internal const string HeritageProjectMainPage = "heritage_project_main_page";
        internal const string HeritageProject = "heritage_project";
        internal const string HeritageProjectDetail = "heritage_project_detail";
        internal const string HeirtageInheritatePeople = "heritage_inheritate_people";

        internal Dictionary<string, MongoDB.Driver.IMongoCollection<BsonDocument>> COLLECTIONS;
        internal readonly IMongoDatabase database;

        private MongodbMain()
        {
            var client = new MongoClient("mongodb://localhost:27017");
            database = client.GetDatabase("heritage");
            List<string> collectionString = new List<string>
            {
                Banner,
                NewsList,
                NewsDetail,
                ForumsList,
                ForumsDetail,
                SpecificTopic,
                SpecificTopicDetail,
                HeritageProjectMainPage,
                HeritageProject,
                HeritageProjectDetail,
                HeirtageInheritatePeople
            };
            COLLECTIONS = new Dictionary<string, IMongoCollection<BsonDocument>>();
            foreach (var collectionName in collectionString)
            {
                COLLECTIONS[collectionName] = database.GetCollection<BsonDocument>(collectionName);
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

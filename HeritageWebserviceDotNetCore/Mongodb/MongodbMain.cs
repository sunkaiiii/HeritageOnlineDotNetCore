using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public sealed class MongodbMain
    {
        private static readonly Lazy<MongodbMain> lazy = new Lazy<MongodbMain>(() => new MongodbMain());
        public static MongodbMain Instance { get { return lazy.Value; } }

        public const string Banner = "banner";
        public const string NewsList = "news_list";
        public const string NewsDetail = "news_detail";
        public const string ForumsList = "forums_list";
        public const string ForumsDetail = "forums_detail";
        public const string SpecificTopic = "specific_topic";
        public const string SpecificTopicDetail = "specific_topic_detail";
        public const string HeritageProjectMainPage = "heritage_project_main_page";
        public const string HeritageProject = "heritage_project";
        public const string HeritageProjectDetail = "heritage_project_detail";
        public const string HeirtageInheritatePeople = "heritage_inheritate_people";
        public const string PeopleMainPage = "people_main_page";

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
                HeirtageInheritatePeople,
                PeopleMainPage
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

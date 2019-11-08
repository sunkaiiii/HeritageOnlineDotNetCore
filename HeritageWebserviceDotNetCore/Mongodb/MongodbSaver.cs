using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public static class MongodbSaver
    {
        private static void InsertMany(IEnumerable<BsonDocument> bsons, string collectionName)
        {
            var collection = MongodbMain.GetCollection(collectionName);
            collection.InsertMany(bsons);
        }

        private static void InsertOne(BsonDocument bson, string collectionName)
        {
            if (bson == null || bson.ElementCount == 0)
                return;
            var collection = MongodbMain.GetCollection(collectionName);
            collection.InsertOne(bson);
        }

        public static void SaveMainpageNewsList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.Banner);
        }

        public static void SaveMainPageNewsList(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.Banner);
        }

        public static void SaveNewsList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.NewsList);
        }
        public static void SaveNewsDetail(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.NewsDetail);
        }

        public static void SaveForumsList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.ForumsList);
        }

        public static void SaveForumsDetail(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.ForumsDetail);
        }

        internal static void SaveSpecificTopicList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.SpecificTopic);
        }

        internal static void SaveHeritageProjectInheritatePeople(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.HeirtageInheritatePeople);
        }

        public static void SaveSpecificTopicDetail(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.SpecificTopicDetail);
        }


        public static void SaveHeritageProjectMainContent(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.HeritageProjectMainPage);
        }

        public static void SaveHeritageProjectNewsList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.HeritageProject);
        }

        public static void SaveHeritageProjectDetail(BsonDocument bsonElements)
        {
            InsertOne(bsonElements, MongodbMain.HeritageProjectDetail);
        }
    }
}

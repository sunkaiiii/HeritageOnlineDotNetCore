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
            InsertMany(bsons, MongodbMain.MAIN_PAGE);
        }

        public static void SaveMainPageNewsList(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.MAIN_PAGE);
        }

        public static void SaveNewsList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.NEWS_LIST);
        }
        public static void SaveNewsDetail(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.NEWS_DETAIL);
        }

        public static void SaveForumsList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.FORUMS_LIST);
        }

        public static void SaveForumsDetail(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.FORUMS_DETAIL);
        }

        internal static void SaveSpecificTopicList(IEnumerable<BsonDocument> bsons)
        {
            InsertMany(bsons, MongodbMain.SPECIFIC_TOPIC);
        }

        public static void SaveSpecificTopicDetail(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.SPECIFIC_TOPIC_DETAIL);
        }


        public static void SaveHeritageProjectMainContent(BsonDocument bson)
        {
            InsertOne(bson, MongodbMain.HERITAGE_PROJECT_MAIN_PAGE);
        }
    }
}

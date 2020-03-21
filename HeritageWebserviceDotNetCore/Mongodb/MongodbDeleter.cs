using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public static class MongodbDeleter
    {
        private delegate bool Deleter(BsonDocument filter);
        private static bool Delete(BsonDocument bson, string collectionName)
        {
            if (bson.ElementCount == 0)
            {
                return false;
            }
            return MongodbMain.GetCollection(collectionName).DeleteMany(bson).DeletedCount>0;
        }
        public static bool DeleteNews(BsonDocument filter)
        {
            return Delete(filter, MongodbMain.NewsList);
        }

        public static bool DeleteNewsDetail(BsonDocument filter)
        {
            return Delete(filter, MongodbMain.NewsDetail); 
        }

        public static bool DeleteForumNews(BsonDocument filter)
        {
            return Delete(filter, MongodbMain.ForumsList);
        }

        public static bool DeleteHeritageProject(BsonDocument filter)
        {
            return Delete(filter, MongodbMain.HeritageProject);
        }

        public static bool DeleteHeritagePeopleList(BsonDocument filter) => Delete(filter, MongodbMain.PeopleList);
        public static bool DeleteHeritagePeopleDetail(BsonDocument filter) => Delete(filter, MongodbMain.PeopleDetail);
    }
}

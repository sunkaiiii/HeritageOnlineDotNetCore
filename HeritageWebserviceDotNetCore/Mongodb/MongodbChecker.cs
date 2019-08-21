using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public class MongodbChecker
    {
        private static bool checkLink(MongoDB.Driver.IMongoCollection<BsonDocument> collection,string url)
        {
            var bson = new BsonDocument();
            bson.Add("link", url);
            return collection.CountDocuments(bson) > 0;
        }
        public static bool CheckNewsExist(string newsUrl)
        {
            var collection = MongodbMain.Instance.Database.GetCollection<BsonDocument>(MongodbMain.NEWS_LIST);
            return checkLink(collection, newsUrl);
        }

        public static bool CheckNewsDetailExist(string newsUrl)
        {
            var collection = MongodbMain.Instance.Database.GetCollection<BsonDocument>(MongodbMain.NEWS_DETAIL);
            return checkLink(collection, newsUrl);
        }

        public static bool CheckForumsListExist(string forumsUrl)
        {
            var collection = MongodbMain.Instance.Database.GetCollection<BsonDocument>(MongodbMain.FORUMS_LIST);
            return checkLink(collection, forumsUrl);
        }

        public static bool CheckForumsDetailExist(string detailUrl)
        {
            var collection = MongodbMain.Instance.Database.GetCollection<BsonDocument>(MongodbMain.FORUMS_DETAIL);
            return checkLink(collection, detailUrl);
        }
    }
}

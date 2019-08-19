﻿using MongoDB.Bson;
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
            var collection = MongodbMain.Instance.database.GetCollection<BsonDocument>(MongodbMain.NEWS_LIST);
            return checkLink(collection, newsUrl);
        }

        public static bool CheckNewsDetailExist(string newsUrl)
        {
            var collection = MongodbMain.Instance.database.GetCollection<BsonDocument>(MongodbMain.NEWS_DETAIL);
            return checkLink(collection, newsUrl);
        }
    }
}

using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public class MongodbChecker
    {
        public static bool CheckNewsExist(string newsUrl)
        {
            var collection = MongodbMain.Instance.database.GetCollection<BsonDocument>(MongodbMain.NEWS_LIST);
            var bson = new BsonDocument();
            bson.Add("link", newsUrl);
            return collection.CountDocuments(bson)>0;
        }
    }
}

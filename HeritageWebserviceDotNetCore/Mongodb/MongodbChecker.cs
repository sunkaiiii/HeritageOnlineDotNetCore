using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Mongodb
{
    public class MongodbChecker
    {
        private static bool checkLink(string collectionName,string url)
        {
            var bson = new BsonDocument();
            var collection = MongodbMain.GetCollection(collectionName);
            bson.Add("link", url);
            return collection.CountDocuments(bson) > 0;
        }

        public static bool CheckMainNewsList(string newsUrl)
        {
            return checkLink(MongodbMain.MAIN_PAGE, newsUrl);
        }

        public static bool CheckNewsExist(string newsUrl)
        {
            return checkLink(MongodbMain.NEWS_LIST, newsUrl);
        }

        public static bool CheckNewsDetailExist(string newsUrl)
        {
            return checkLink(MongodbMain.NEWS_DETAIL, newsUrl);
        }

        public static bool CheckForumsListExist(string forumsUrl)
        {
            return checkLink(MongodbMain.FORUMS_LIST, forumsUrl);
        }

        public static bool CheckForumsDetailExist(string detailUrl)
        {
            return checkLink(MongodbMain.FORUMS_DETAIL, detailUrl);
        }

        public static bool CheckSpecialListNewsListExist(string newsList)
        {
            return checkLink(MongodbMain.SPECIFIC_TOPIC, newsList);
        }
        public static bool CheckSpecialListNewsDetailExist(string detailUrl)
        {
            return checkLink(MongodbMain.SPECIFIC_TOPIC_DETAIL, detailUrl);
        }

        internal static bool CheckHeritageProjectInheritatePeopleExist(string url)
        {
            return checkLink(MongodbMain.HEIRTAGE_INHERITATE_PEOPLE,url);
        }

        public static bool CheckHeritageProjectExist(string url)
        {
            return checkLink(MongodbMain.HERITAGE_PROJECT, url);
        }

        public static bool CheckHeritageProjectDetailExist(string detailUrl)
        {
            return checkLink(MongodbMain.HERITAGE_PROJECT_DETAIL, detailUrl);
        }
    }
}

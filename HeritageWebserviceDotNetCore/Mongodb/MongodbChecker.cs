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
            return checkLink(MongodbMain.Banner, newsUrl);
        }

        public static bool CheckNewsExist(string newsUrl)
        {
            return checkLink(MongodbMain.NewsList, newsUrl);
        }

        public static bool CheckNewsDetailExist(string newsUrl)
        {
            return checkLink(MongodbMain.NewsDetail, newsUrl);
        }

        public static bool CheckForumsListExist(string forumsUrl)
        {
            return checkLink(MongodbMain.ForumsList, forumsUrl);
        }

        public static bool CheckForumsDetailExist(string detailUrl)
        {
            return checkLink(MongodbMain.ForumsDetail, detailUrl);
        }

        public static bool CheckSpecialListNewsListExist(string newsList)
        {
            return checkLink(MongodbMain.SpecificTopic, newsList);
        }
        public static bool CheckSpecialListNewsDetailExist(string detailUrl)
        {
            return checkLink(MongodbMain.SpecificTopicDetail, detailUrl);
        }

        internal static bool CheckHeritageProjectInheritatePeopleExist(string url)
        {
            return checkLink(MongodbMain.HeirtageInheritatePeople,url);
        }

        public static bool CheckHeritageProjectExist(string url)
        {
            return checkLink(MongodbMain.HeritageProject, url);
        }

        public static bool CheckHeritageProjectDetailExist(string detailUrl)
        {
            return checkLink(MongodbMain.HeritageProjectDetail, detailUrl);
        }
    }
}

using HeritageWebserviceDotNetCore.Mongodb;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Tests
{
    class MongodbTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private BsonDocument CreateEmptyBson()
        {
            return new BsonDocument();
        }

        private BsonDocument CreateTestBson()
        {
            var bson = new BsonDocument();
            bson.Add("link", GetTestLink());
            return bson;
        }

        private BsonDocument CreateBTestBson()
        {
            var bson = new BsonDocument();
            bson.Add("link", GetBTestLink());
            return bson;
        }


        private string GetTestLink()
        {
            return "/23456776543/345678765432/34567765432/56765432";
        }

        private string GetBTestLink()
        {
            return "/2432432/43242/3432/4/324";
        }

        [Test]
        public void TestNewsListChecker()
        {
            var bson = CreateTestBson();
            var insertList = new List<BsonDocument>();
            insertList.Add(bson);
            MongodbSaver.SaveNewsList(insertList);
            var url1 = @"/news_details/18987.html";
            var url2 = @"/news_details/184987.html";
            var shouldBeTrue = MongodbChecker.CheckNewsExist(url1);
            var shouldBeFalse = MongodbChecker.CheckNewsExist(url2);
            Assert.IsTrue(shouldBeTrue);
            Assert.IsFalse(shouldBeFalse);
            var result = MongodbDeleter.DeleteNews(bson);
            Assert.IsTrue(result);
            Assert.False(MongodbDeleter.DeleteNews(CreateEmptyBson()));
            shouldBeTrue = MongodbChecker.CheckNewsExist(url1);
            Assert.IsTrue(shouldBeTrue);
        }

        [Test]
        public void TestNewsDetailChecker()
        {
            var bson = CreateTestBson();
            Assert.IsFalse(MongodbChecker.CheckForumsDetailExist(GetTestLink()));
            MongodbSaver.SaveNewsDetail(bson);
            Assert.IsTrue(MongodbDeleter.DeleteNewsDetail(bson));
            Assert.False(MongodbDeleter.DeleteNewsDetail(CreateEmptyBson()));
            Assert.IsFalse(MongodbChecker.CheckNewsDetailExist(GetTestLink()));
        }

        [Test]
        public void TestForumsChecker()
        {
            var bson = CreateTestBson();
            var insertList = new List<BsonDocument>();
            insertList.Add(bson);
            MongodbSaver.SaveForumsList(insertList);
            Assert.IsTrue(MongodbChecker.CheckForumsListExist(GetTestLink()));
            Assert.IsFalse(MongodbChecker.CheckForumsListExist(GetBTestLink()));
            Assert.IsTrue(MongodbDeleter.DeleteForumNews(bson));
            Assert.IsFalse(MongodbDeleter.DeleteForumNews(CreateEmptyBson()));
            Assert.IsFalse(MongodbDeleter.DeleteForumNews(CreateBTestBson()));
            Assert.IsFalse(MongodbChecker.CheckForumsListExist(GetTestLink()));
        }
    }
}

using HeritageWebserviceDotNetCore.Mongodb;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceDotNetCore.Tests
{
    class MongodbCheckerTests
    {
        [SetUp]
        public void Setup()
        {
        }

        private BsonDocument createTestBson()
        {
            var bson = new BsonDocument();
            bson.Add("link", getTestLink());
            return bson;
        }

        private string getTestLink()
        {
            return "/23456776543/345678765432/34567765432/56765432";
        }

        [Test]
        public void TestNewsListChecker()
        {
            var bson = createTestBson();
            var insertList = new List<BsonDocument>();
            insertList.Add(bson);
            MongodbMain.Instance.SaveNewsList(insertList);
            var url1 = @"/news_details/18987.html";
            var url2 = @"/news_details/184987.html";
            var shouldBeTrue = MongodbChecker.CheckNewsExist(url1);
            var shouldBeFalse = MongodbChecker.CheckNewsExist(url2);
            Assert.IsTrue(shouldBeTrue);
            Assert.IsFalse(shouldBeFalse);
            var result = MongodbMain.Instance.DeleteNews(bson);
            Assert.IsTrue(result);
            shouldBeTrue = MongodbChecker.CheckNewsExist(url1);
            Assert.IsTrue(shouldBeTrue);
        }

        [Test]
        public void TestNewsDetailChecker()
        {
            var bson = createTestBson();
            Assert.IsFalse(MongodbChecker.CheckForumsDetailExist(getTestLink()));
            MongodbMain.Instance.SaveNewsDetail(bson);
            Assert.IsTrue(MongodbMain.Instance.DeleteNewsDetail(bson));
            Assert.IsFalse(MongodbChecker.CheckNewsDetailExist(getTestLink()));
        }
    }
}

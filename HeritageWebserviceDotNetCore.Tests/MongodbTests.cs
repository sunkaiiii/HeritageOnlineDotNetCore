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


        private string GetTestLink()=> "/23456776543/345678765432/34567765432/56765432";
        private string GetBTestLink() => "/2432432/43242/3432/4/324";

        private void TestMongodbListCollection(Action<List<BsonDocument>> addAction,Func<BsonDocument,bool> deleteAction,Func<string,bool> checkAction)
        {
            var insertList = new List<BsonDocument>
            {
                CreateTestBson(),
                CreateBTestBson()
            };
            addAction(insertList);
            Assert.IsTrue(checkAction(GetTestLink()));
            Assert.IsTrue(checkAction(GetBTestLink()));
            Assert.IsTrue(deleteAction(CreateTestBson()));
            Assert.IsFalse(deleteAction(CreateEmptyBson()));
            Assert.IsFalse(checkAction(GetTestLink()));
            Assert.IsTrue(deleteAction(CreateBTestBson()));
            Assert.IsFalse(checkAction(GetBTestLink()));
            Assert.IsFalse(deleteAction(CreateBTestBson()));
        }

        private void TestMongodbSingleDocument(Action<BsonDocument> addAction, Func<BsonDocument, bool> deleteAction, Func<string, bool> checkAction)
        {
            var bson = CreateTestBson();
            Assert.IsFalse(checkAction(GetTestLink()));
            addAction(bson);
            Assert.IsTrue(checkAction(GetTestLink()));
            Assert.IsTrue(deleteAction(bson));
            Assert.IsFalse(checkAction(GetTestLink()));
        }

        [Test]
        public void TestForumsChecker()=>TestMongodbListCollection(MongodbSaver.SaveForumsList, MongodbDeleter.DeleteForumNews, MongodbChecker.CheckForumsListExist);

        [Test]
        public void TestHeritageProject()=>TestMongodbListCollection(MongodbSaver.SaveHeritageProjectNewsList, MongodbDeleter.DeleteHeritageProject, MongodbChecker.CheckHeritageProjectExist);
        
        [Test]
        public void TestPeopleListPage()=> TestMongodbListCollection(MongodbSaver.SavePeopleListInformation, MongodbDeleter.DeleteHeritagePeopleList, MongodbChecker.CheckPeoplePageListExist);

        [Test]
        public void TestNewsListChecker() => TestMongodbListCollection(MongodbSaver.SaveNewsList, MongodbDeleter.DeleteNews, MongodbChecker.CheckNewsExist);

        [Test]
        public void TestNewsDetailChecker() => TestMongodbSingleDocument(MongodbSaver.SaveNewsDetail, MongodbDeleter.DeleteNewsDetail, MongodbChecker.CheckNewsDetailExist);

        [Test]
        public void TestPeopleDetailChecker() => TestMongodbSingleDocument(MongodbSaver.SavePeopleDetailInformation, MongodbDeleter.DeleteHeritagePeopleDetail, MongodbChecker.CheckPeopleDetailExist);
    }
}

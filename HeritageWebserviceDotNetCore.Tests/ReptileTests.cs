using HeritageWebserviceReptileDotNetCore.Reptile;
using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceRetileDotNetCore.Tests
{
    class ReptileTests
    {
        [SetUp]
        public void Setup()
        { 
        }

        [Test]
        public void TestGetHeritageMapTableInformation()
        {
            var bson = new BsonDocument();
            GetHeritageProjects.GetHeritageMapTableInformation(bson);
            Console.WriteLine(bson);
            Assert.IsTrue(bson.ElementCount > 0);
            Assert.IsTrue(((bson.GetElement(0).Value) as BsonArray).Count == 2);
        }
    }
}

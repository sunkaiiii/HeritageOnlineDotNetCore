using HeritageWebserviceDotNetCore.Reptile;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace HeritageWebserviceDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            GetIhChina.StartReptile() ;
            //GetForumsWorker.GetForumsList();
            //GetSpecialTopicWorker.GetSpecialTopic();
            // GetNewsDetail.GenerateNewsDetail(@"http://www.ihchina.cn/news_details/18992.html");
            //var client = new MongoClient("mongodb://localhost:27017");
            //var database = client.GetDatabase("heritage");
            //var collection = database.GetCollection<BsonDocument>("heritage");
            //var document = new BsonDocument
            //{
            //    {"name","test2" },
            //    {"type","haha" },
            //    {"info",new BsonDocument
            //    {
            //        {"x",203 },
            //        {"y",102 }
            //    } }
            //};
            //collection.InsertOne(document);

            //var documents = Enumerable.Range(0, 100).Select(i => new BsonDocument("counter", i));
            //collection.InsertMany(documents);
            //var count = collection.CountDocuments(new BsonDocument());
            //Console.WriteLine(count);
            //var document2 = collection.Find(new BsonDocument()).FirstOrDefault();
            //Console.WriteLine(document2);
            //var documents2 = collection.Find(new BsonDocument()).ToList();
            //documents2.ForEach(doc => Console.WriteLine(doc));

        }
    }
}

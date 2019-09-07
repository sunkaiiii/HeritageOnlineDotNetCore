using System;
using System.Text;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceDotNetCore.Reptile;
using HeritageWebserviceReptileDotNetCore.Mongodb;
using MongoDB.Bson;

namespace HeritageWebserviceReptileDotNetCore.Reptile
{
    public static class GetHeritageProjects
    {
        private static readonly string PROJECT_MAIN_PAGE = "http://www.ihchina.cn/project.html";

        public static void GetHeritageProject()
        {
            var doc = WebpageHelper.GetHttpRequestDocument(PROJECT_MAIN_PAGE);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='x-wrap']/div[@class='title']/div");
            if(nodes!=null)
            {
                var bson = new BsonDocument();
                foreach(var node in nodes)
                {
                    switch(node.Attributes["class"].Value)
                    {
                        case "h30":
                            bson.Add("title", node.InnerText);
                            break;
                        case "p":
                            StringBuilder sb = new StringBuilder();
                            foreach (var contentNode in node.ChildNodes)
                            {
                                sb.Append(contentNode.InnerText);
                            }
                            bson.Add("content", sb.ToString());
                            break;

                    }
                }

                nodes = doc.DocumentNode.SelectNodes("//div/div[@class='num-item']");
                if(nodes != null)
                {
                    var bsonArray = new BsonArray();
                    foreach(var node in nodes)
                    {
                        var numBson = new BsonDocument();
                        foreach (var childNode in node.ChildNodes)
                        {
                            switch (childNode.Attributes["class"]?.Value)
                            {
                                case "b":
                                    numBson.Add("num", childNode.Attributes["data-rn"].Value);
                                    break;
                                case "h18":
                                    numBson.Add("desc", childNode.InnerText);
                                    break;
                            }
                        }
                        bsonArray.Add(numBson);
                    }

                    bson.Add("numItem", bsonArray);
                }

                var mongodbBson = MongodbGetter.GetHeritageProjectMainPageDesc();
                if(mongodbBson==null)
                {
                    Console.WriteLine("Insert Heritage Project Content");
                    MongodbSaver.SaveHeritageProjectMainContent(bson);
                }
                else if(!CheckBsonIsEqual(bson,mongodbBson))
                {
                    Console.WriteLine("Update Heritage Project Content");
                    MongodbUpdater.UpdateHeritageProjectMainContent(bson);
                }
                else
                {
                    Console.WriteLine("Not Insert Heritage Project Content");
                    return;
                }
                Console.WriteLine(bson.ToString());
            }
        }

        private static bool CheckBsonIsEqual(BsonDocument bson, BsonDocument mongodbBson)
        {
            var bsonArray = bson.GetElement("numItem").Value;
            var mongodbArray = mongodbBson.GetElement("numItem").Value;
            if(bsonArray.IsBsonArray && mongodbArray.IsBsonArray)
            {
                return bsonArray.Equals(mongodbArray);
            }
            else
            {
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceDotNetCore.Reptile;
using HeritageWebserviceReptileDotNetCore.Mongodb;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace HeritageWebserviceReptileDotNetCore.Reptile
{
    public static class GetHeritageProjects
    {
        private static readonly string PROJECT_MAIN_PAGE = "http://www.ihchina.cn/project.html";
        private static readonly string REQUEST_URL = "http://www.ihchina.cn/Article/Index/getProject.html?province=&rx_time=&type=&cate=&keywords=&category_id=16&limit=10&p={0}";

        struct HeritageProjectRequest
        {
            public int More { get; set; }
            public PageInfo Links { get; set; }
            public HeritageProject[] List { get; set; }

        }

        struct PageInfo
        {
            public int Total_pages { get; set; }
            public int Current { get; set; }
        }

        struct HeritageProject
        {
            public string Id { get; set; }
            public string Auto_id { get; set; } //项目序号
            public string Num { get; set; } //编号
            public string Title { get; set; } //名称
            public string Type { get; set; } //类别
            public string Reg_type { get; set; }
            public string Rx_time { get; set; } //公布时间
            public string Project_num { get; set; }
            public string Cate { get; set; } //类型
            public string Province { get; set; } //申报地区或单位
            public string City { get; set; }
            public string Content { get; set; }
            public string Playtype { get; set; }
            public string Unit { get; set; }
            public string Link { get; set; }
        }

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
                }
                Console.WriteLine(bson);
                GetAllProjectList();
            }
        }

        private static void GetAllProjectList()
        {
            short errorTime = 0;
            var totalPages = 10;
            var block = new BufferBlock<string>();
            var task = GetHeritageProjectDetailWorker.GenerateProjectDetailPage(block);
            for (int i=1;i<totalPages;i++)
            {
                if(errorTime>10)
                {
                    Console.WriteLine("GetAllProjectList: reach the limitation of error time");
                    break;
                }
                var currentPage = String.Format(REQUEST_URL, i);
                Console.WriteLine("Starting process: "+currentPage);
                var requestResult = WebpageHelper.GetRequest(currentPage);
                if(string.IsNullOrEmpty(requestResult))
                {
                    errorTime++;
                    continue;
                }
                var jsonObject = JsonConvert.DeserializeObject<HeritageProjectRequest>(requestResult);
                if(jsonObject.Links.Total_pages!=totalPages)
                {
                    totalPages = jsonObject.Links.Total_pages;
                }
                var list = jsonObject.List;
                if(list==null || list.Length==0)
                {
                    continue;
                }
                var bsonArray = new List<BsonDocument>();
                var heritageType = typeof(HeritageProject);
                var properties = typeof(HeritageProject).GetProperties();
                for (int j=0;j<list.Length;j++)
                {
                    var bsonDocument= new BsonDocument();
                    list[j].Link= "/project_details/" + list[j].Id;
                    foreach(var property in properties)
                    {
                        //反射获取HeritageProject所有属性
                        //以属性名作为MongoDB存储的Key值
                        //反射获取对应List当中的值
                        bsonDocument.Add(property.Name.ToLower(), Regex.Replace(heritageType.GetProperty(property.Name).GetValue(list[j]).ToString(),"<.*?>",string.Empty));
                    }

                    if(MongodbChecker.CheckHeritageProjectExist(list[j].Link))
                    {
                        Console.WriteLine("Duplicated Heritage Project Link: {0}", list[j].Link);
                        errorTime++;
                        continue;
                    }
                    block.Post(list[j].Link);
                    bsonArray.Add(bsonDocument);
                }

                
                if(bsonArray.Count!=0)
                {
                    MongodbSaver.SaveHeritageProjectNewsList(bsonArray);
                }
                
                if (jsonObject.More!=1)
                {
                    Console.WriteLine("GetAllProjectList: current page is {0}, more equals 0", i);
                    break;
                }
            }
            block.Complete();
            task.Wait();
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

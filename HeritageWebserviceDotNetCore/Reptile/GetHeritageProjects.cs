using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceDotNetCore.Reptile;
using HeritageWebserviceReptileDotNetCore.Mongodb;
using HtmlAgilityPack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using Newtonsoft.Json;

namespace HeritageWebserviceReptileDotNetCore.Reptile
{
    public static class GetHeritageProjects
    {
        private static readonly string PROJECT_MAIN_PAGE = "http://www.ihchina.cn/project.html";
        private static readonly string REQUEST_URL = "http://www.ihchina.cn/Article/Index/getProject.html?province=&rx_time=&type=&cate=&keywords=&category_id=16&limit=10&p={0}";
        private static readonly string MAIN_PAGE = "http://www.ihchina.cn/";
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
            GetMainPageInformation();
            GetAllProjectList();
        }

        private static void GetMainPageInformation()
        {
            var doc = WebpageHelper.GetHttpRequestDocument(PROJECT_MAIN_PAGE);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='x-wrap']/div[@class='title']/div");
            if (nodes != null)
            {
                var bson = new BsonDocument();
                foreach (var node in nodes)
                {
                    switch (node.Attributes["class"].Value)
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

                //非遗页面的数字
                nodes = doc.DocumentNode.SelectNodes("//div/div[@class='num-item']");
                if (nodes != null)
                {
                    var bsonArray = new BsonArray();
                    foreach (var node in nodes)
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

                //获取首页的非物质文化遗产地图
                GetHeritageMapTableInformation(bson);

                var mongodbBson = MongodbGetter.GetHeritageProjectMainPageDesc();
                if (mongodbBson == null)
                {
                    Console.WriteLine("Insert Heritage Project Content");
                    MongodbSaver.SaveHeritageProjectMainContent(bson);
                }
                else if (!CheckBsonIsEqual(bson, mongodbBson))
                {
                    Console.WriteLine("Update Heritage Project Content");
                    MongodbUpdater.UpdateHeritageProjectMainContent(bson);
                }
                else
                {
                    Console.WriteLine("Not Insert Heritage Project Content");
                }
                Console.WriteLine(bson);
            }
        }

        public static void GetHeritageMapTableInformation(BsonDocument bson)
        {
            var doc = WebpageHelper.GetHttpRequestDocument(MAIN_PAGE);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='tab-cont map_num']/div");
            if (nodes != null)
            {
                var tableArray = new BsonArray();
                for (int i = 0; i < 2; i++)
                {
                    var tableInfo = new BsonDocument();
                    string name;
                    switch (i)
                    {
                        case 0:
                            name = "国家级代表项目";
                            break;
                        case 1:
                            name = "国家级代表性传承人";
                            break;
                        default:
                            name = "";
                            break;
                    }
                    var node = nodes[i];
                    BsonDocument tableInforamtionNode = GetMapInformation(node);
                    if (tableInforamtionNode != null && tableInforamtionNode.ElementCount > 0)
                    {
                        tableInfo.Add("desc", name);
                        tableInfo.Add("content", tableInforamtionNode);
                        tableArray.Add(tableInfo);
                    }
                }
                bson.Add("mapTables", tableArray);
            }
        }

        private static BsonDocument GetMapInformation(HtmlNode node)
        {
            var mapInformationBson = new BsonDocument();
            var descNodes = node.SelectNodes(".//div[@class='p_mor']"); //描述信息
            if (descNodes != null)
            {
                var descBsons = new BsonArray();
                foreach (var desc in descNodes)
                {
                    descBsons.Add(desc.InnerText);
                }
                mapInformationBson.Add("desc", descBsons);
            }
            //表格
            var tableNodes = node.SelectNodes(".//div[@class='table']/div[@class='td']");
            if (tableNodes == null)
            {
                tableNodes = node.SelectNodes(".//div[@class='table']/div[@class='td ']"); //网站两个表格的class名字不一样……
            }
            if (tableNodes != null)
            {
                var tableBsons = new BsonArray();
                foreach (var td in tableNodes)
                {
                    var tdBson = new BsonDocument();
                    var linkNode = td.SelectSingleNode(".//a");
                    var numNode = linkNode.SelectSingleNode(".//div[@class='num']");
                    var descNode = linkNode.SelectSingleNode(".//div[@class='p']");
                    if (numNode != null && descNode != null)
                    {
                        tdBson.Add("num", numNode.InnerText);
                        tdBson.Add("desc", descNode.InnerText);
                    }
                    var link = linkNode.Attributes["href"].Value;
                    var searchLink = link.Substring(link.IndexOf('?'));
                    if (searchLink.IndexOf('#') > 0)
                    {
                        searchLink = searchLink.Remove(searchLink.IndexOf('#'));
                    }
                    tdBson.Add("searchLink", searchLink);
                    tableBsons.Add(tdBson);
                }
                mapInformationBson.Add("table", tableBsons);
            }

            //总计
            var total = node.SelectSingleNode(".//div[@class='total']");
            if (total != null)
            {
                mapInformationBson.Add("total", total.InnerText);
            }
            return mapInformationBson;
        }

        private static void GetAllProjectList()
        {
            short errorTime = 0;
            var totalPages = DebugHelper.DebugHelperTools.IsDebugMode() ? 2 : 10;
            var block = new BufferBlock<string>();
            var task = GetHeritageProjectDetailWorker.GenerateProjectDetailPage(block);
            for (int i = 1; i < totalPages; i++)
            {
                if (errorTime > 10)
                {
                    Console.WriteLine("GetAllProjectList: reach the limitation of error time");
                    break;
                }
                var currentPage = String.Format(REQUEST_URL, i);
                Console.WriteLine("Starting process: " + currentPage);
                var requestResult = WebpageHelper.GetRequest(currentPage);
                if (string.IsNullOrEmpty(requestResult))
                {
                    errorTime++;
                    continue;
                }
                var jsonObject = JsonConvert.DeserializeObject<HeritageProjectRequest>(requestResult);
                if (jsonObject.Links.Total_pages != 0
                    && jsonObject.Links.Total_pages != totalPages
                    && !DebugHelper.DebugHelperTools.IsDebugMode())
                {
                    totalPages = jsonObject.Links.Total_pages;
                }
                var list = jsonObject.List;
                if (list == null || list.Length == 0)
                {
                    continue;
                }
                var bsonArray = new List<BsonDocument>();
                var heritageType = typeof(HeritageProject);
                var properties = typeof(HeritageProject).GetProperties();
                for (int j = 0; j < list.Length; j++)
                {
                    var bsonDocument = new BsonDocument();
                    list[j].Link = "/project_details/" + list[j].Id;
                    foreach (var property in properties)
                    {
                        //反射获取HeritageProject所有属性
                        //以属性名作为MongoDB存储的Key值
                        //反射获取对应List当中的值
                        bsonDocument.Add(property.Name.ToLower(), Regex.Replace(heritageType.GetProperty(property.Name).GetValue(list[j]).ToString(), "<.*?>", string.Empty));
                    }

                    if (MongodbChecker.CheckHeritageProjectExist(list[j].Link))
                    {
                        Console.WriteLine("Duplicated Heritage Project Link: {0}", list[j].Link);
                        errorTime++;
                        continue;
                    }
                    block.Post(list[j].Link);
                    bsonArray.Add(bsonDocument);
                }


                if (bsonArray.Count != 0)
                {
                    MongodbSaver.SaveHeritageProjectNewsList(bsonArray);
                }

                if (jsonObject.More != 1)
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
            mongodbBson.Remove("_id");
            return mongodbBson.Equals(bson);
        }
    }
}

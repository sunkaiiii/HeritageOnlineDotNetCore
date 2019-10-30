using HeritageWebserviceDotNetCore.Mongodb;
using HtmlAgilityPack;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceReptileDotNetCore.Reptile;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class GetIhChina
    {
        internal const string MAIN_PAGE = "http://www.ihchina.cn";
        internal const string NEWS_LIST_URL = MAIN_PAGE + "/Article/Index/getList.html";
        private static readonly HttpClient client = new HttpClient();
        private Dictionary<String, String> classificaton = new Dictionary<string, string> { { "新闻动态", "u11" }, { "论坛", "u12" }, { "专题报道", "u13" } };
        public static void StartReptile()
        {
            var imageTargetBlock = WebImageSaver.Instance.ImageTargetBlock;
            var imageSaverTask = WebImageSaver.Instance.SaveFileAsync(imageTargetBlock);
            getMainPageList();
            Task<int> result = GetNewsListWorker.GetNewsList(imageSaverTask, imageTargetBlock); //仅用一个线程去获取新闻内容，另外一个线程取图片
            Console.WriteLine("Get news list returns {0}, task is over", result.Result);
            GetForumsWorker.GetForumsList();
            GetSpecialTopicWorker.GetSpecialTopic();
            GetHeritageProjects.GetHeritageProject();
            Console.WriteLine("All processes have been completed, now waiting for image downloading...");
            imageTargetBlock.Complete();
            Console.WriteLine("image task returns {0}, task is over", imageSaverTask.Result);
            Console.WriteLine("Image downloading has been finished");
        }

        private static void getMainPageList()
        {
            var doc = new HtmlDocument();
//#if DEBUG
           // doc.Load(@".\mainPageHtml.html");
//#else
            var url = MAIN_PAGE;
            var web = new HtmlWeb();
            doc = web.Load(url);
//#endif
            IEnumerable<BsonDocument> nodes = from links in doc.DocumentNode.Descendants()
                                              where
                                              links.Name == "a"
                                              && links.Attributes["href"] != null
                                              && links.InnerText.Trim().Length > 0
                                              && links.Attributes["href"].Value.Contains("news_details")
                                              && !MongodbChecker.CheckMainNewsList(links.Attributes["href"].Value)
                                              select new BsonDocument()
                                              .Add("link", links.Attributes["href"].Value)
                                              .Add("text", links.InnerText)
                                              .Add("date", links.ParentNode.ParentNode.FirstChild.InnerText);
            //TODO 第一新闻有图片页，且格式不同，需要适配
            if (nodes != null && nodes.Count()>0)
            {
                MongodbSaver.SaveMainpageNewsList(nodes);
                foreach (var node in nodes)
                {
                    Console.WriteLine(node["url"].AsBsonValue + " " + node["text"].AsBsonValue + " " + node["date"].AsBsonValue);
                }
            }


        }
    }
}

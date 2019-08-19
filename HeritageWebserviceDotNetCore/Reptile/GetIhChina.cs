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
           // getMainPageList();
            var newsListTask =GetNewsListWorker.GetNewsList(imageSaverTask, imageTargetBlock);
            newsListTask.Wait();
            imageTargetBlock.Complete();
            imageSaverTask.Wait();
        }

        private static void getMainPageList()
        {
            var doc = new HtmlDocument();
#if DEBUG
            doc.Load(@".\mainPageHtml.html");
#else
            var url = mainPage;
            var web = new HtmlWeb();
            doc = web.Load(url);
#endif
            IEnumerable<BsonDocument> nodes = from links in doc.DocumentNode.Descendants()
                                              where links.Name == "a" && links.Attributes["href"] != null && links.InnerText.Trim().Length > 0 && links.Attributes["href"].Value.Contains("news_details")
                                              select new BsonDocument().Add("url", links.Attributes["href"].Value).Add("text", links.InnerText).Add("date", links.ParentNode.ParentNode.FirstChild.InnerText);
            //TODO 第一新闻有图片页，且格式不同，需要适配
            MongodbMain.Instance.SaveMainpageNewsList(nodes);
            foreach (var node in nodes)
            {
                Console.WriteLine(node["url"].AsBsonValue + " " + node["text"].AsBsonValue + " " + node["date"].AsBsonValue);
            }
        }
    }
}

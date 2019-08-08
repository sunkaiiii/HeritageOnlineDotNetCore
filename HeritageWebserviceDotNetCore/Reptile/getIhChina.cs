using HeritageWebserviceDotNetCore.Mongodb;
using HtmlAgilityPack;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HeritageWebserviceDotNetCore.Reptile
{
    class GetIhChina
    {
        private const String mainPage = "http://www.ihchina.cn/";
        private Dictionary<String, String> classificaton = new Dictionary<string, string> { { "新闻动态", "u11" }, { "论坛", "u12" },{ "专题报道", "u13" } };
        
        public static void StartReptile()
        {
            getMainPageList();
            getNewsList();
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
            foreach(var node in nodes)
            {
                Console.WriteLine(node["url"].AsBsonValue+" "+node["text"].AsBsonValue+" "+node["date"].AsBsonValue);
            }
        }

        public static void SaveMainPageHtml()
        {
            var web = new HtmlWeb();
            var doc= web.Load(mainPage);
            using(StreamWriter file=new StreamWriter(@".\mainPageHtml.html"))
            {
                doc.Save(file);
            }
        }

        public static void getNewsList()
        {
            
        }
    }
}

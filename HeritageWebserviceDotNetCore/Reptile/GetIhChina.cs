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
        private static BufferBlock<string> newsDetailTargetBlock = new BufferBlock<string>();
        private Dictionary<String, String> classificaton = new Dictionary<string, string> { { "新闻动态", "u11" }, { "论坛", "u12" }, { "专题报道", "u13" } };

        public static void StartReptile()
        {
            getMainPageList();
            GetNewsList();
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

        public static void SaveMainPageHtml()
        {
            var web = new HtmlWeb();
            var doc = web.Load(MAIN_PAGE);
            using (StreamWriter file = new StreamWriter(@".\mainPageHtml.html"))
            {
                doc.Save(file);
            }
        }

        struct NewsListResponse
        {
            public string data;
            public int more;
        };
      
        public static void GetNewsList()
        {
            int errorTime = 0;
            var newsDetailPageGenerate = GetNewsDetail.GenerateNewsDetail(newsDetailTargetBlock);
            var imageTargetBlock = WebImageSaver.Instance.ImageTargetBlock;
            var imageSaverTask = WebImageSaver.Instance.SaveFileAsync(imageTargetBlock);
            for (int page = 1; page < 2; page++)
            {
                if (errorTime > 10)
                {
                    Console.WriteLine("reach the limitation of error time");
                    break;
                }
                Console.WriteLine("starting process {0} page", page);
                string pageURL = String.Format("{0}?category_id=9&page={1}&limit=0", NEWS_LIST_URL, page);
                var result = WebpageHelper.GetRequest(pageURL);
                var jsonObject = JsonConvert.DeserializeObject<NewsListResponse>(result);
                if(jsonObject.more!=1&&String.IsNullOrEmpty(jsonObject.data))
                {
                    break;
                }
                var doc = new HtmlDocument();
                doc.LoadHtml(jsonObject.data);
                var newsListNodes = from links in doc.DocumentNode.Descendants()
                                    where links.Name == "div" && links.Attributes["class"] != null && links.Attributes["class"].Value == "list-item"
                                    select links;
                var newslistBsons = new List<BsonDocument>();
                foreach (var node in newsListNodes)
                {
                    var newsBason = new BsonDocument();
                    var titleNode = node.SelectSingleNode(".//div[@class='h16']/a");
                    if (titleNode != null)
                    {
                        var link = titleNode.Attributes["href"].Value;
                        //if (MongodbChecker.CheckNewsExist(link))
                        //{
                        //    errorTime++;
                        //    continue;
                        //}
                        newsDetailTargetBlock.Post(MAIN_PAGE + link);
                        newsBason.Add("link", link);
                        newsBason.Add("title", titleNode.Attributes["title"].Value);
                    }
                    var imgNode = node.SelectSingleNode(".//img");
                    if (imgNode != null)
                    {
                        newsBason.Add("image", imgNode.Attributes["src"].Value);

                        imageTargetBlock.Post(MAIN_PAGE+imgNode.Attributes["src"].Value);
                    }
                    var dataNode = node.SelectSingleNode(".//div[@class='date']/div");
                    if(dataNode!=null)
                    {
                        newsBason.Add("date", dataNode.InnerText);
                    }
                    var contentNode = node.SelectSingleNode(".//div[@class='p']");
                    if(contentNode!=null)
                    {
                        newsBason.Add("content", contentNode.InnerText);
                    }
                    if(newsBason.Count()!=0)
                    {
                        newslistBsons.Add(newsBason);
                    }
                }
                if(newslistBsons.Count!=0)
                {
                    MongodbMain.Instance.SaveNewsList(newslistBsons);
                }
            }
            newsDetailTargetBlock.Complete();
            newsDetailPageGenerate.Wait();
            imageTargetBlock.Complete();
            imageSaverTask.Wait();
        }

    }
}

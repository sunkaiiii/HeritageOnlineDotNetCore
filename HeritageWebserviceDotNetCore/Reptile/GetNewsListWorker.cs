using HeritageWebserviceDotNetCore.Mongodb;
using HtmlAgilityPack;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace HeritageWebserviceDotNetCore.Reptile
{
    class GetNewsListWorker
    {
        struct NewsListResponse
        {
            public string data;
            public int more;
        };

        public static async Task<int> GetNewsList(Task<int> imageSaverTask, BufferBlock<string> imageTargetBlock)
        {
           BufferBlock<string> newsDetailTargetBlock = new BufferBlock<string>();
           int errorTime = 0;
            var newsDetailPageGenerate = GetNewsDetail.GenerateNewsDetail(newsDetailTargetBlock);
            for (int page = 1; page < 2; page++)
            {
                if (errorTime > 10)
                {
                    Console.WriteLine("reach the limitation of error time");
                    break;
                }
                Console.WriteLine("starting process {0} page", page);
                string pageURL = String.Format("{0}?category_id=9&page={1}&limit=0", GetIhChina.NEWS_LIST_URL, page);
                var result = WebpageHelper.GetRequest(pageURL);
                var jsonObject = JsonConvert.DeserializeObject<NewsListResponse>(result);
                if (jsonObject.more != 1 && String.IsNullOrEmpty(jsonObject.data))
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
                        if (MongodbChecker.CheckNewsExist(link))
                        {
                            errorTime++;
                            continue;
                        }
                        newsDetailTargetBlock.Post(GetIhChina.MAIN_PAGE + link);
                        newsBason.Add("link", link);
                        newsBason.Add("title", titleNode.Attributes["title"].Value);
                    }
                    var imgNode = node.SelectSingleNode(".//img");
                    if (imgNode != null)
                    {
                        newsBason.Add("image", imgNode.Attributes["src"].Value);
                        imageTargetBlock.Post(GetIhChina.MAIN_PAGE + imgNode.Attributes["src"].Value);
                    }
                    var dataNode = node.SelectSingleNode(".//div[@class='date']/div");
                    if (dataNode != null)
                    {
                        newsBason.Add("date", dataNode.InnerText);
                    }
                    var contentNode = node.SelectSingleNode(".//div[@class='p']");
                    if (contentNode != null)
                    {
                        newsBason.Add("content", contentNode.InnerText);
                    }
                    if (newsBason.Count() != 0)
                    {
                        newslistBsons.Add(newsBason);
                    }
                }
                if (newslistBsons.Count != 0)
                {
                    MongodbSaver.SaveNewsList(newslistBsons);
                }
            }
            newsDetailTargetBlock.Complete();
            return await newsDetailPageGenerate;
        }
    }
}

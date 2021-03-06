﻿using HeritageWebserviceDotNetCore.Mongodb;
using HtmlAgilityPack;
using MongoDB.Bson;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceReptileDotNetCore.Reptile;
using HeritageWebserviceReptileDotNetCore.DebugHelper;

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
            var newsDetailPageGenerate = GetNewsDetail.GenerateNewsDetail(newsDetailTargetBlock, imageTargetBlock);
            int pageNumber = DebugHelperTools.IsDebugMode() ? 2 : 255;
            for (int page = 1; page < pageNumber; page++)
            {
                if (errorTime > 10)
                {
                    Console.WriteLine("reach the limitation of error time");
                    break;
                }
                Console.WriteLine("starting process {0} page", page);
                string pageURL = String.Format("{0}?category_id=9&page={1}&limit=0", GetIhChina.NewsListUrl, page);
                var result = WebpageHelper.GetRequest(pageURL);
                var jsonObject = JsonConvert.DeserializeObject<NewsListResponse>(result);
                if (jsonObject.more != 1 && String.IsNullOrEmpty(jsonObject.data))
                {
                    Console.WriteLine("GetNewsList: There is no more here, break from the loop");
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
                    var newsBson = new BsonDocument();
                    var titleNode = node.SelectSingleNode(".//div[@class='h16']/a");
                    if (titleNode != null)
                    {
                        var link = titleNode.Attributes["href"].Value;
                        if (MongodbChecker.CheckNewsExist(link))
                        {
                            Console.WriteLine("duplicated url: page {0}", link);
                            errorTime++;
                            continue;
                        }
                        newsDetailTargetBlock.Post(link);
                        newsBson.Add("link", link);
                        newsBson.Add("title", titleNode.Attributes["title"].Value);
                    }
                    var imgNode = node.SelectSingleNode(".//img");
                    if (imgNode != null)
                    {
                        var imgUrl = WebpageHelper.GetSubUrl(imgNode.Attributes["src"].Value);
                        newsBson.Add("img", imgUrl);
                        newsBson.Add("compressImg",WebImageSaver.Instance.GetComressImageName(imgUrl));
                        imageTargetBlock.Post(GetIhChina.MainPage + imgNode.Attributes["src"].Value);
                    }
                    var dataNode = node.SelectSingleNode(".//div[@class='date']/div");
                    if (dataNode != null)
                    {
                        newsBson.Add("date", dataNode.InnerText);
                    }
                    var contentNode = node.SelectSingleNode(".//div[@class='p']");
                    if (contentNode != null)
                    {
                        newsBson.Add("content", contentNode.InnerText);
                    }
                    if (newsBson.Count() != 0)
                    {
                        newslistBsons.Add(newsBson);
                    }
                    //每10条进行一次数据库插入，减少内存负担
                    if (newslistBsons.Count == 10)
                    {
                        MongodbSaver.SaveNewsList(newslistBsons);
                        for(int i=0;i<10;i++)
                        {
                            newslistBsons[i] = null;
                        }
                        newslistBsons.Clear();
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

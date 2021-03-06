﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceDotNetCore.Reptile;
using HtmlAgilityPack;
using MongoDB.Bson;

namespace HeritageWebserviceReptileDotNetCore.Reptile
{
    public class GetIhChina
    {
        internal const string MainPage = "http://www.ihchina.cn";
        internal const string NewsListUrl = MainPage + "/Article/Index/getList.html";
        private static readonly HttpClient Client = new HttpClient();

        private Dictionary<string, string> classificaton = new Dictionary<string, string>
            {{"新闻动态", "u11"}, {"论坛", "u12"}, {"专题报道", "u13"}};

        public static async Task StartReptile()
        {
            var imageTargetBlock = WebImageSaver.Instance.InitImageBlock(); //初始化图片序列
            var imageSaverTask = WebImageSaver.Instance.SaveFileAsync(imageTargetBlock);
            GetBanner(imageTargetBlock);
            var result = GetNewsListWorker.GetNewsList(imageSaverTask, imageTargetBlock); //仅用一个线程去获取新闻内容，
            Console.WriteLine("Get news list returns {0}, task is over", await result);
            GetForumsWorker.GetForumsList(imageTargetBlock);
            GetSpecialTopicWorker.GetSpecialTopic(imageTargetBlock);
            GetHeritageProjects.GetHeritageProject(imageTargetBlock);
            await GetPeoplePage.StartGetPeoplePage(imageTargetBlock);
            Console.WriteLine("All processes have been completed, now waiting for image downloading...");
            imageTargetBlock.Complete();
            Console.WriteLine("image task returns {0}, task is over", await imageSaverTask);
            Console.WriteLine("Image downloading has been finished");
        }

        private static void GetBanner(BufferBlock<string> imageTargetBlock)
        {
            var doc = WebpageHelper.GetHttpRequestDocument(MainPage);
            var nodes = from links in doc.DocumentNode.Descendants()
                        where
                            links.Name == "a"
                            && links.Attributes["href"] != null
                            && links.Attributes["class"] != null
                            && links.Attributes["class"].Value.Equals("slick-link p-show")
                            && !MongodbChecker.CheckMainNewsList(links.Attributes["href"].Value)
                        select new BsonDocument()
                            .Add("link", links.Attributes["href"].Value)
                            .Add("img",WebpageHelper.GetSubUrl(
                               links.Attributes["style"].Value.Substring(
                                    links.Attributes["style"].Value.IndexOf("/", StringComparison.Ordinal),
                                    links.Attributes["style"].Value.LastIndexOf(")", StringComparison.Ordinal) -
                                    links.Attributes["style"].Value.IndexOf("/", StringComparison.Ordinal))))
                            .Add("compressImg",WebImageSaver.Instance.GetComressImageName(WebpageHelper.GetSubUrl(
                               links.Attributes["style"].Value.Substring(
                                    links.Attributes["style"].Value.IndexOf("/", StringComparison.Ordinal),
                                    links.Attributes["style"].Value.LastIndexOf(")", StringComparison.Ordinal) -
                                    links.Attributes["style"].Value.IndexOf("/", StringComparison.Ordinal)))))
                            .Add("originImage", links.Attributes["style"].Value.Substring(
                                    links.Attributes["style"].Value.IndexOf("/", StringComparison.Ordinal),
                                    links.Attributes["style"].Value.LastIndexOf(")", StringComparison.Ordinal) -
                                    links.Attributes["style"].Value.IndexOf("/", StringComparison.Ordinal)));
            foreach (var node in nodes)
            {
                imageTargetBlock.Post(node["originImage"].AsBsonValue.ToString());
            }

            var enumerable = nodes as BsonDocument[] ?? nodes.ToArray();
            if (!enumerable.Any()) return;
            MongodbSaver.SaveMainpageNewsList(nodes);
            foreach (var node in enumerable)
            {
                Console.WriteLine(node["link"].AsBsonValue + " " + node["img"].AsBsonValue);
            }
        }
    }
}
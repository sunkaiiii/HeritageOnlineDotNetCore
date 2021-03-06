﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MongoDB.Bson;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceReptileDotNetCore.Reptile;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public static class GetNewsDetail
    {
        public readonly static int DUPLICATED_NEWS = 1;
        public readonly static int PROCESS_SUCCESS = 0;
        private delegate bool Checker(string url);
        private delegate void Saver(BsonDocument bson);
        public static async Task<int> GenerateNewsDetail(ISourceBlock<string> urlSource, BufferBlock<string> block)
        {
            Checker checker = MongodbChecker.CheckNewsDetailExist;
            Saver saver = MongodbSaver.SaveNewsDetail;
            return await processBufferBlock(urlSource, checker, saver,block);
        }
        public static async Task<int> GenerateForumDetail(ISourceBlock<string> urlSource, BufferBlock<string> block)
        {
            Checker checker = MongodbChecker.CheckForumsDetailExist;
            Saver saver = MongodbSaver.SaveForumsDetail;
            return await processBufferBlock(urlSource, checker, saver,block);
        }

        internal static async Task<int> GenerateSpecificTopicDetail(ISourceBlock<string> urlSource, BufferBlock<string> block)
        {
            Checker checker = MongodbChecker.CheckSpecialListNewsDetailExist;
            Saver saver = MongodbSaver.SaveSpecificTopicDetail;
            return await processBufferBlock(urlSource, checker, saver,block);
        }

        internal static async Task<int> GeneratePeopleDetail(ISourceBlock<string> urlSource, BufferBlock<string> block)
        {
            Checker checker = MongodbChecker.CheckPeopleDetailExist;
            Saver saver = MongodbSaver.SavePeopleDetailInformation;
            return await processBufferBlock(urlSource, checker, saver,block);
        }

        private static async Task<int> processBufferBlock(ISourceBlock<string> urlSource, Checker checker, Saver saver, BufferBlock<string> block)
        {
            var errorTime = 0;
            while (await urlSource.OutputAvailableAsync())
            {
                if(errorTime==10)
                {
                    return DUPLICATED_NEWS;
                }
                var url = urlSource.Receive();
                if (checker(url))
                {
                    Console.WriteLine("duplicated url: {0}", url);
                    errorTime++;
                    continue;
                }
                var bson = processDetailPage(url, block);
                if (bson != null)
                {
                    saver(bson);
                }
            }
            return PROCESS_SUCCESS; ;
        }

        private static BsonDocument bson = new BsonDocument();
        private static BsonDocument processDetailPage(string url, BufferBlock<string> imageBlock)
        {
            Console.WriteLine("starting process: " + url + "....");
            var doc = WebpageHelper.GetHttpRequestDocument(url);
            var titleNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-title']");
            if (titleNode == null)
                return null;
            bson.Clear();
            bson.Add("link", url);
            var titleNameNode = titleNode.SelectSingleNode(".//div[@class='h24']");
            if (titleNameNode != null)
            {
                bson.Add("title", titleNameNode.InnerText.Replace("\t", "")); //规格化文字，TODO 未完成
            }
            var titleSubItemsNodes = titleNode.SelectNodes(".//div[@class='sub']/span[@class='sub-item']");
            if(titleSubItemsNodes==null)
            {
                return null;
            }
            var subTitleList = new BsonArray();
            foreach (var subItemNode in titleSubItemsNodes)
            {
                if (subItemNode.SelectSingleNode(".//div[@class='en']") != null) //日期的要单独处理
                {
                    var dateNode = subItemNode.SelectSingleNode(".//div[@class='en'");
                    subTitleList.Add(dateNode.InnerText);
                }
                else
                {
                    subTitleList.Add(subItemNode.InnerText);
                }
            }
            bson.Add("subtitle", subTitleList);

            //开始读取内容
            var contentList = new BsonArray();
            var contentNodes = doc.DocumentNode.SelectNodes("//div[@class='article-cont']/p");
            if(contentNodes==null)
            {
                return null;
            }
            foreach (var node in contentNodes)
            {
                var lineDic = new BsonDocument();
                var picNode = node.SelectSingleNode(".//img");
                if (picNode != null)
                {
                    var imgUrl = WebpageHelper.GetSubUrl(picNode.Attributes["src"].Value);
                    lineDic["type"] = "img";
                    lineDic["content"] = imgUrl;
                    lineDic["compressImg"] = WebImageSaver.Instance.GetComressImageName(imgUrl);
                    imageBlock.Post(GetIhChina.MainPage + picNode.Attributes["src"].Value);
                }
                else
                {
                    lineDic["type"] = "text";
                    lineDic["content"] = node.InnerText;
                }
                contentList.Add(lineDic);
            }
            bson.Add("content", contentList);
            var authorNode = doc.DocumentNode.SelectSingleNode("//div[@class='author']");
            if (authorNode != null)
            {
                bson.Add("author", authorNode.InnerText);
            }
            //爬取底部相关阅读
            var relativeNews = doc.DocumentNode.SelectNodes("//div[@class='list-mod4']/a");
            if (relativeNews != null)
            {
                var relativeNewsList = new BsonArray();
                foreach (var node in relativeNews)
                {
                    var newsDic = new BsonDocument();
                    newsDic["link"] = node.Attributes["href"].Value;
                    var date = node.SelectSingleNode(".//div[@class='date']");
                    if (date != null)
                    {
                        newsDic["date"] = date.InnerText;
                    }
                    var title = node.SelectSingleNode(".//div[@class='p']");
                    if (title != null)
                    {
                        newsDic["title"] = title.InnerText;
                    }
                    relativeNewsList.Add(newsDic);
                }
                if (relativeNewsList.Count > 0)
                {
                    bson.Add("relativeNews", relativeNewsList);
                }
            }
            Console.WriteLine(bson);
            return bson;
        }


    }
}

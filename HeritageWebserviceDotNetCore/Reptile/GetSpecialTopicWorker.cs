﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceReptileDotNetCore.DebugHelper;
using MongoDB.Bson;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class GetSpecialTopicWorker
    {
        public static void GetSpecialTopic(BufferBlock<string> imageTargetBlock)
        {
            var block = new BufferBlock<string>();
            var task = GetNewsDetail.GenerateSpecificTopicDetail(block,imageTargetBlock);
            int errorTime = 0;
            string firstPage = "http://www.ihchina.cn/news_1/p/1.html";
            var lastPageNumber = DebugHelperTools.IsDebugMode()?2:WebpageHelper.GetPageLastIndex(firstPage);
            for (int i = 1; i < lastPageNumber && errorTime < 10; i++)
            {
                var listUrl = String.Format("http://www.ihchina.cn/news_1/p/{0}.html", i);
                Console.WriteLine("starting process page:{0}", listUrl);
                var doc = WebpageHelper.GetHttpRequestDocument(listUrl);
                var listNodes = doc.DocumentNode.SelectNodes("//div[@class='list-mod3']/div[@class='list-item']");
                if (listNodes == null)
                {
                    continue;
                }
                List<BsonDocument> result = new List<BsonDocument>();
                foreach (var node in listNodes)
                {
                    var bson = WebpageHelper.AnalizeGeneralListInformation(node, MongodbChecker.CheckSpecialListNewsListExist, imageTargetBlock);
                    if (bson == null)
                    {
                        errorTime++;
                        Console.WriteLine("duplicated url: page {0}", i);
                    }
                    if (bson != null)
                    {
                        var link = bson.GetValue("link").ToString();
                        block.Post(link);
                        result.Add(bson);
                    }
                    //每10条进行一次数据库插入，减少内存负担
                    if (result.Count == 10)
                    {
                        MongodbSaver.SaveSpecificTopicList(result);
                        result.Clear();
                    }
                }
                if (result.Count > 0)
                {
                    MongodbSaver.SaveSpecificTopicList(result);
                }
            }
            block.Complete();
            task.Wait();
        }
    }
}

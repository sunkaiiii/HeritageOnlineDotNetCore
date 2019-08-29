﻿using HeritageWebserviceDotNetCore.Mongodb;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace HeritageWebserviceDotNetCore.Reptile
{
    class GetForumsWorker
    {
        public static void GetForumsList()
        {
            var block = new BufferBlock<string>();
            var task = GetNewsDetail.GenerateForumDetail(block);
            for(int i=1;i<255;i++)
            {
                var listUrl = String.Format("http://www.ihchina.cn/luntan/p/{0}.html", i);
                Console.WriteLine("starting process page:{0}", listUrl);
                var doc = WebpageHelper.getHttpRequestDocument(listUrl);
                var listNodes = doc.DocumentNode.SelectNodes("//div[@id='datalist']/div[@class='list-item']");
                if (listNodes == null)
                {
                    continue ;
                }
                List<BsonDocument> result = new List<BsonDocument>();
                foreach(var node in listNodes)
                {
                    var bson = WebpageHelper.AnalizeGeneralListInformation(node, MongodbChecker.CheckForumsDetailExist);
                    if (bson != null)
                    {
                        var link = bson.GetElement("link").ToString();
                        block.Post(GetIhChina.MAIN_PAGE + link);
                        result.Add(bson);
                    }
                }
                if (result.Count > 0)
                {
                    MongodbSaver.SaveForumsList(result);
                }
            }
            block.Complete();
            task.Wait();
        }
    }
}

using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceReptileDotNetCore.DebugHelper;
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
            int errorTime = 0;
            var firstPage = "http://www.ihchina.cn/luntan/p/1.html";
            var lastPageNumber = DebugHelperTools.IsDebugMode() ? 2 : WebpageHelper.GetPageLastIndex(firstPage);
            for (int i = 1; i < lastPageNumber && errorTime < 10; i++)
            {
                var listUrl = String.Format("http://www.ihchina.cn/luntan/p/{0}.html", i);
                Console.WriteLine("starting process page:{0}", listUrl);
                var doc = WebpageHelper.GetHttpRequestDocument(listUrl);
                var listNodes = doc.DocumentNode.SelectNodes("//div[@id='datalist']/div[@class='list-item']");
                if (listNodes == null)
                {
                    errorTime++;
                    continue;
                }
                List<BsonDocument> result = new List<BsonDocument>();
                foreach (var node in listNodes)
                {
                    if (errorTime == 10)
                    {
                        break;
                    }
                    var bson = WebpageHelper.AnalizeGeneralListInformation(node, MongodbChecker.CheckForumsListExist);
                    if (bson == null)
                    {
                        errorTime++;
                        Console.WriteLine("duplicated url: page {0}", i);
                        continue;
                    }
                    if (bson != null)
                    {
                        var link = bson.GetElement("link").ToString();
                        block.Post(link);
                        result.Add(bson);
                    }
                    //每10条进行一次数据库插入，减少内存负担
                    if (result.Count == 10)
                    {
                        MongodbSaver.SaveForumsList(result);
                        result.Clear();
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

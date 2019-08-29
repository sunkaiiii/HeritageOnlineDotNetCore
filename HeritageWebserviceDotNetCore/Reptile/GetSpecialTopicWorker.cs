using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceDotNetCore.Mongodb;
using MongoDB.Bson;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class GetSpecialTopicWorker
    {
        public static void GetSpecialTopic()
        {
            var block = new BufferBlock<string>();
            var task = GetNewsDetail.GenerateSpecificTopicDetail(block);
            for(int i=1;i<255;i++)
            {
                var listUrl = String.Format("http://www.ihchina.cn/news_1/p/{0}.html", i);
                Console.WriteLine("starting process page:{0}", listUrl);
                var doc = WebpageHelper.getHttpRequestDocument(listUrl);
                var listNodes = doc.DocumentNode.SelectNodes("//div[@class='list-mod3']/div[@class='list-item']");
                if(listNodes==null)
                {
                    continue;
                }
                List<BsonDocument> result = new List<BsonDocument>();
                foreach(var node in listNodes)
                {
                    var bson = WebpageHelper.AnalizeGeneralListInformation(node, (url)=>false);
                    if (bson != null)
                    {
                        var link = bson.GetValue("link").ToString();
                        block.Post(GetIhChina.MAIN_PAGE + link);
                        result.Add(bson);
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

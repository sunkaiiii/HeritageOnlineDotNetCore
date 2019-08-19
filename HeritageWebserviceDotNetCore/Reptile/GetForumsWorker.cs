using HeritageWebserviceDotNetCore.Mongodb;
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
            for(int i=3;i<4;i++)
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
                    var bson = new BsonDocument();
                    var linkNode = node.SelectSingleNode(".//a");
                    if(linkNode!=null)
                    {
                        if(MongodbChecker.CheckForumsListExist(linkNode.Attributes["href"].Value))
                        {
                            continue;
                        }
                        var link = linkNode.Attributes["href"].Value;
                        bson.Add("link",link);
                        bson.Add("title", linkNode.InnerText);
                        block.Post(GetIhChina.MAIN_PAGE+link);
                    }
                    var dateNode = node.SelectSingleNode(".//div[@class='date']");
                    if(dateNode!=null)
                    {
                        bson.Add("date", dateNode.InnerText);
                    }
                    var contentNode = node.SelectSingleNode(".//div[@class='p']");
                    if(contentNode!=null)
                    {
                        bson.Add("content", contentNode.InnerText);
                    }
                    var imageNode = node.SelectSingleNode(".//img");
                    if(imageNode!=null)
                    {
                        bson.Add("img", imageNode.Attributes["src"].Value);
                    }
                    result.Add(bson);
                    Console.WriteLine(bson.ToString());
                }
                if (result.Count > 0)
                {
                    MongodbMain.Instance.SaveForumsList(result);
                }
            }
            block.Complete();
            task.Wait();
        }
    }
}

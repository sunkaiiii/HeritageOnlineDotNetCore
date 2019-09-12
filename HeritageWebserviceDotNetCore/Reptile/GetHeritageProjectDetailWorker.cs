using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceDotNetCore.Reptile;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace HeritageWebserviceReptileDotNetCore.Reptile
{
    static class GetHeritageProjectDetailWorker
    {
        public readonly static int DUPLICATED_NEWS = 1;
        public readonly static int PROCESS_SUCCESS = 0;
        private static BsonDocument bsonDocument = new BsonDocument();
        public static async Task<int> GenerateProjectDetailPage(ISourceBlock<string> urlSource)
        {
            var errTime = 0;
            while(await urlSource.OutputAvailableAsync())
            {
                if(errTime == 10)
                {
                    return DUPLICATED_NEWS;
                }
                var url = urlSource.Receive();
                if(MongodbChecker.CheckHeritageProjectDetailExist(url))
                {
                    errTime++;
                    continue;
                }
                var bson = GenerateHeritageProjectDetailPage(url);
                if(bson != null)
                {
                    MongodbSaver.SaveHeritageProjectDetail(bson);
                }
            }
            return PROCESS_SUCCESS;
        }

        private static BsonDocument GenerateHeritageProjectDetailPage(string url)
        {
            var doc = WebpageHelper.GetHttpRequestDocument(url);
            var containerNode = doc.DocumentNode.SelectSingleNode("//div[@class='x-container']");
            if (containerNode == null)
            {
                return null;
            }

            //头部标题和表格
            var titleNode = containerNode.SelectSingleNode("//div[@class='t_head']/div[@class='h30']");
            if (titleNode == null)
            {
                return null;
            }
            bsonDocument.Clear();
            bsonDocument.Add("title", titleNode.InnerText);
            bsonDocument.Add("link", url);
            var tableNodes = containerNode.SelectSingleNode(".//div[@class='table']").SelectNodes(".//div[@class='p']");
            if (tableNodes != null)
            {
                BsonArray descList = new BsonArray();
                foreach (var node in tableNodes.Descendants())
                {
                    descList.Add(node.InnerText);
                }
                if (descList.Count > 0)
                {
                    bsonDocument.Add("desc", descList);
                }
            }
            //中央区域描述文字
            var contentNode = containerNode.SelectSingleNode(".//div[@class='text']//div[@class='p']");
            if (contentNode != null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var node in contentNode.Descendants())
                {
                    sb.Append(node.InnerText);
                }
                bsonDocument.Add("text", sb.ToString());
            }

            //底部相关传承人
            GetBottomInheritageAndReleventInfo(containerNode, bsonDocument);
            Console.WriteLine(bsonDocument);
            return bsonDocument;
        }

        private static void GetBottomInheritageAndReleventInfo(HtmlAgilityPack.HtmlNode containerNode,BsonDocument bsonDocument)
        {
            var inheritageNode = containerNode.SelectNodes(".//div[@class='inherit_xx2']/div/div");
            if (inheritageNode != null)
            {
                for (int i = 0; i < inheritageNode.Count; i += 2)
                {
                    var node = inheritageNode[i];
                    if (node.Attributes["class"].Value == "tit")
                    {
                        var nextNode = inheritageNode[i + 1];
                        var tableRows = nextNode.SelectNodes(".//tr");

                        if (tableRows != null)
                        {
                            var rowArray = new BsonArray();
                            foreach (var row in tableRows)
                            {
                                var colums = row.SelectNodes(".//td");
                                if (colums == null)
                                {
                                    continue;
                                }
                                //抽出每一行的数据
                                var columnBsonDocument = new BsonDocument();
                                var columnArray = new BsonArray();
                                foreach (var column in colums)
                                {
                                    BsonDocument columnBson = new BsonDocument();
                                    var name = column.SelectSingleNode(".//div").InnerText;
                                    columnBson.Add("key", name);
                                    columnBson.Add("value", column.InnerText.Replace(name, string.Empty));
                                    var link = column.SelectSingleNode(".//a"); //找到哪一行出现了对应项目的链接
                                    if(link!=null)
                                    {
                                        columnBsonDocument.Add("link", link.Attributes["href"].Value);
                                    }
                                    columnArray.Add(columnBson);
                                }
                                columnBsonDocument.Add("content", columnArray);
                                rowArray.Add(columnBsonDocument);
                            }
                            if (rowArray != null)
                            {
                                if (node.InnerText.Trim().Contains("传承人"))
                                {
                                    bsonDocument.Add("inheritate", rowArray);
                                }
                                else
                                {
                                    bsonDocument.Add("ralevant", rowArray);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using MongoDB.Bson;
using HeritageWebserviceDotNetCore.Mongodb;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public static class GetNewsDetail
    {
        public static async Task<int> GenerateNewsDetail(ISourceBlock<string> urlSource)
        {
            BufferBlock<string> block = WebImageSaver.Instance.ImageTargetBlock;
            while(await urlSource.OutputAvailableAsync())
            {
                var doc = WebpageHelper.getHttpRequestDocument(urlSource.Receive());
                var titleNode = doc.DocumentNode.SelectSingleNode("//div[@class='article-title']");
                if (titleNode == null)
                    continue;
                var bson = new BsonDocument();
                var titleNameNode = titleNode.SelectSingleNode(".//div[@class='h24']");
                if (titleNameNode != null)
                {
                    bson.Add("title", titleNameNode.InnerText.Replace("\t", "")); //规格化文字，TODO 未完成
                }
                var titleSubItemsNodes = titleNode.SelectNodes(".//div[@class='sub']/span[@class='sub-item']");
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
                foreach (var node in contentNodes)
                {
                    var lineDic = new BsonDocument();
                    var picNode = node.SelectSingleNode(".//img");
                    if (picNode != null)
                    {
                        lineDic["img"] = picNode.Attributes["src"].Value;
                        block.Post(GetIhChina.MAIN_PAGE + picNode.Attributes["src"].Value);
                    }
                    else {
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
                    if(relativeNewsList.Count>0)
                    {
                        bson.Add("relativeNews", relativeNewsList);
                    }
                }
                Console.WriteLine(bson);
                MongodbMain.Instance.SaveNewsDetail(bson);
            }
            return 1;
        }
    }
}

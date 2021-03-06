﻿using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceReptileDotNetCore.Mongodb;
using HeritageWebserviceReptileDotNetCore.Reptile;
using HtmlAgilityPack;
using MongoDB.Bson;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public static class WebpageHelper
    {
        private static HtmlWeb htmlWeb = new HtmlWeb();
        public static string GetSubUrl(string url)
        {
            var result = url;
            if (url.Contains(@"/"))
            {
                result = url.Substring(url.LastIndexOf(@"/") + 1);
            }
            StringBuilder urlBuilder = new StringBuilder(result);
            foreach (char invalidChar in Path.GetInvalidFileNameChars())
            {
                urlBuilder = urlBuilder.Replace(invalidChar.ToString(), String.Empty);
            }
            return urlBuilder.ToString();
        }

        public static string CorrectRequestString(string url)
        {
            if (!url.StartsWith(GetIhChina.MainPage)) //当连接不是以MAIN_PAGE开头的时候，为期添加HOST_NAME
            {
                if (!url.StartsWith(@"/")) //有些时候，解析的link会添加奇怪的字符，剪裁到第一个链接出现
                {
                    url = url.Substring(url.IndexOf(@"/"));
                }
                url = GetIhChina.MainPage + url;
            }
            return url;
        }

        public static string GetRequest(string url)
        {
#if DEBUG
            if (WebPageSaver.CheckCacheFileExist(url))
            {
                return WebPageSaver.GetSimpleRequestResult(url);
            }
#endif
            var request = WebRequest.Create(url);
            request.Method = "GET";
            var result = "";
            try
            {
                var responseStream = request.GetResponse().GetResponseStream();
                result = new StreamReader(responseStream).ReadToEnd().ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return result;
            }
#if DEBUG
            WebPageSaver.SaveSimpleRequestResult(url, result);
#endif
            return result;
        }

        public static HtmlAgilityPack.HtmlDocument GetHttpRequestDocument(string url)
        {
#if DEBUG
            if (WebPageSaver.CheckCacheFileExist(url))
            {
                return WebPageSaver.GetHtmlDocument(url);
            }
#endif
            url = CorrectRequestString(url);
            HtmlDocument doc;
            try
            {
                doc = htmlWeb.Load(url);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HtmlDocument();
            }
#if DEBUG
            WebPageSaver.SaveHtml(url, doc);
#endif
            return doc;
        }

        public delegate bool Checker(string url);
        public static BsonDocument AnalizeGeneralListInformation(HtmlAgilityPack.HtmlNode node, Checker checker,BufferBlock<string> imageTargetBlock)
        {
            var bson = new BsonDocument();
            var linkNode = node.SelectSingleNode(".//div[@class='h16']/a");
            if (linkNode != null)
            {
                if (checker(linkNode.Attributes["href"].Value))
                {
                    return null;
                }
                var link = linkNode.Attributes["href"].Value;
                bson.Add("link", link);
                bson.Add("title", linkNode.InnerText);
            }
            var dateNode = node.SelectSingleNode(".//div[@class='date']");
            if (dateNode != null)
            {
                bson.Add("date", dateNode.InnerText);
            }
            var contentNode = node.SelectSingleNode(".//div[@class='p']");
            if (contentNode != null)
            {
                bson.Add("content", contentNode.InnerText);
            }
            var imageNode = node.SelectSingleNode(".//img");
            if (imageNode != null)
            {
                var imgUrl = imageNode.Attributes["src"].Value;
                bson.Add("img", GetSubUrl(imgUrl));
                bson.Add("compressImg", WebImageSaver.Instance.GetComressImageName(GetSubUrl(imgUrl)));
                imageTargetBlock.Post(GetIhChina.MainPage + imgUrl);
            }
            Console.WriteLine(bson.ToString());
            return bson;
        }

        public static int GetPageLastIndex(string pageUrl)
        {
            var doc = WebpageHelper.GetHttpRequestDocument(pageUrl);
            var nodes = doc.DocumentNode.SelectNodes("//div[@class='page-mod']/ul/li");
            if (nodes == null)
            {
                return 100;
            }
            int lastIndex = 0;
            foreach (var node in nodes)
            {
                string indexString = node.InnerText;
                int tempIndex;
                if (int.TryParse(indexString, out tempIndex))
                {
                    if (lastIndex < tempIndex)
                    {
                        lastIndex = tempIndex;
                    }
                }
            }
            return lastIndex;
        }

        public static bool TryToInsertOrUpdateABson(BsonDocument bsonDocument, string collectionName) => TryToInsertOrUpdateABson(bsonDocument, collectionName, new BsonDocument());
        public static bool TryToInsertOrUpdateABson(BsonDocument bsomDocument, string collectionName, BsonDocument condition)
        {
            var mongodbBson = MongodbGetter.GetACollection(collectionName);
            if (mongodbBson == null)
            {
                MongodbSaver.InsertOne(bsomDocument, collectionName);
                return true;
            }
            else if (!CheckBsonIsEqual(bsomDocument, mongodbBson))
            {
                MongodbUpdater.UpdateACollection(bsomDocument, collectionName, condition);
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool CheckBsonIsEqual(BsonDocument bson, BsonDocument mongodbBson)
        {
            mongodbBson.Remove("_id");
            return mongodbBson.Equals(bson);
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks.Dataflow;
using HtmlAgilityPack;
using MongoDB.Bson;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public static class WebpageHelper
    {
        private  static HtmlWeb htmlWeb=new HtmlWeb();
        public static string GetSubUrl(string url)
        {
            var result = url;
            if (url.Contains(@"/"))
            {
                result = url.Substring(url.LastIndexOf(@"/")+1);
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
            if (!url.StartsWith(GetIhChina.MAIN_PAGE)) //当连接不是以MAIN_PAGE开头的时候，为期添加HOST_NAME
            {
                if (!url.StartsWith(@"/")) //有些时候，解析的link会添加奇怪的字符，剪裁到第一个链接出现
                {
                    url = url.Substring(url.IndexOf(@"/"));
                }
                url = GetIhChina.MAIN_PAGE + url;
            }
            return url;
        }

        public static string GetRequest(string url)
        {
#if DEBUG
            if(File.Exists(WebpageHelper.GetSubUrl(url)))
            {
                return WebPageSaver.GetSimpleRequestResult(url);
            }
#endif
            var request = WebRequest.Create(url);
            request.Method = "GET";
            var responseStream = request.GetResponse().GetResponseStream();
            var result = new StreamReader(responseStream).ReadToEnd().ToString();
#if DEBUG
            WebPageSaver.SaveSimpleRequestResult(url,result);
#endif
            return result;
        }

        public static HtmlAgilityPack.HtmlDocument getHttpRequestDocument(string url)
        {
#if DEBUG
            if(File.Exists(WebpageHelper.GetSubUrl(url)))
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
            catch(Exception e)
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
        public static BsonDocument AnalizeGeneralListInformation(HtmlAgilityPack.HtmlNode node,Checker checker)
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
                bson.Add("img", imgUrl);
                WebImageSaver.Instance.ImageTargetBlock.Post(GetIhChina.MAIN_PAGE+imgUrl);
            }
            Console.WriteLine(bson.ToString());
            return bson;
        }
    }
}

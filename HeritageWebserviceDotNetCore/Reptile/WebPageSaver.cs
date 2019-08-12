using System;
using System.IO;
using HtmlAgilityPack;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class WebPageSaver
    { 
        public static void SaveHtml(string url, HtmlAgilityPack.HtmlDocument doc)
        {
            var saveFileName = WebpageHelper.GetSubUrl(url);
            using (StreamWriter file = new StreamWriter(saveFileName))
            {
                doc.Save(file);
            }
        }

        public static HtmlAgilityPack.HtmlDocument GetHtmlDocument(string url)
        {
            var saveFileName = WebpageHelper.GetSubUrl(url);
            using(StreamReader fileStreamReader=new StreamReader(saveFileName))
            {
                var response = fileStreamReader.ReadToEnd();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response);
                return doc;
            }
        }

        public static void SaveSimpleRequestResult(string url,string resultString)
        {
            var saveFileName = WebpageHelper.GetSubUrl(url);
            using (StreamWriter file=new StreamWriter(saveFileName))
            {
                file.Write(resultString);
            }
        }
        
        public static string GetSimpleRequestResult(string url)
        {
            string result;
            using (StreamReader file = new StreamReader(WebpageHelper.GetSubUrl(url)))
            {
                result = file.ReadToEnd();
            }
            return result;
        }
    }
}

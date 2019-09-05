using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HtmlAgilityPack;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class WebPageSaver
    { 
        public static void SaveHtml(string url, HtmlAgilityPack.HtmlDocument doc)
        {
            var saveFileName = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), WebpageHelper.GetSubUrl(url));
            using (StreamWriter file = new StreamWriter(saveFileName))
            {
                doc.Save(file);
            }
        }

        public static HtmlAgilityPack.HtmlDocument GetHtmlDocument(string url)
        {
            var saveFileName = WebPageSaver.GetCacheFileName(url);
            using(StreamReader fileStreamReader=new StreamReader(saveFileName))
            {
                var response = fileStreamReader.ReadToEnd();
                var doc = new HtmlAgilityPack.HtmlDocument();
                doc.LoadHtml(response);
                return doc;
            }
        }

        public static bool CheckCacheFileExist(string url)
        {
            return File.Exists(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), WebpageHelper.GetSubUrl(url)));
        }

        public static string GetCacheFileName(string url)
        {
            return Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), WebpageHelper.GetSubUrl(url));
        }

        public static string GetCacheImageName(string imageURL)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "img", WebpageHelper.GetSubUrl(imageURL));
        }

        public static void SaveSimpleRequestResult(string url,string resultString)
        {
            if(!Directory.Exists("cache"))
            {
                Directory.CreateDirectory("cache");
            }
            var saveFilePath = GetCacheFileName(url);
            using (StreamWriter file=new StreamWriter(saveFilePath))
            {
                file.Write(resultString);
            }
        }
        
        public static string GetSimpleRequestResult(string url)
        {
            string result;
            using (StreamReader file = new StreamReader(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "cache"), WebpageHelper.GetSubUrl(url))))
            {
                result = file.ReadToEnd();
            }
            return result;
        }

        public  static async Task<int> SaveFileAsync(ISourceBlock<string> source)
        {
            while (await source.OutputAvailableAsync())
            {
                WebClient wc = new WebClient();
                if (!Directory.Exists("img"))
                {
                    Directory.CreateDirectory("img");
                }
                var imageUrl = source.Receive();
                var savePath = GetCacheImageName(imageUrl);
                wc.DownloadFile(imageUrl, savePath);
            }
            return 1;
        }
    }
}

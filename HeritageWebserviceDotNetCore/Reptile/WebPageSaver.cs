﻿using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HtmlAgilityPack;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class WebPageSaver
    {
        private HtmlWeb htmlWeb = new HtmlWeb();
        public static void SaveHtml(string url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var saveFileName = WebpageHelper.GetSubUrl(url);
            using (StreamWriter file = new StreamWriter(saveFileName))
            {
                doc.Save(file);
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
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "img", WebpageHelper.GetSubUrl(imageUrl));
                wc.DownloadFile(imageUrl, savePath);
            }
            return 1;
        }
    }
}

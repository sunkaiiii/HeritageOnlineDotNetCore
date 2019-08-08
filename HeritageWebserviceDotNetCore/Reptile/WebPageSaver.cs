using System;
using System.IO;
using HtmlAgilityPack;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class WebPageSaver
    {
        private HtmlWeb htmlWeb = new HtmlWeb();
        public static void SaveHtml(String url)
        {
            var web = new HtmlWeb();
            var doc = web.Load(url);
            var saveFileName = WebpageHelper.GetSubUrl(url);
            using (StreamWriter file = new StreamWriter(saveFileName))
            {
                doc.Save(file);
            }
        }
        
    }
}

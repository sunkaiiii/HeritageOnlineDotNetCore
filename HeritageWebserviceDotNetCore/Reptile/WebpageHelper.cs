using System;
using System.IO;
using System.Net;
using System.Text;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public static class WebpageHelper
    {
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
    }
}

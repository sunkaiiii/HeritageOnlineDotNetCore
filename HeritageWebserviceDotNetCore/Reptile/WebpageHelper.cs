using System;
namespace HeritageWebserviceDotNetCore.Reptile
{
    public static class WebpageHelper
    {
        public static String GetSubUrl(String url)
        {
            var result = url;
            if (url.Contains(@"/"))
            {
                result = url.Substring(url.LastIndexOf(@"/")+1);
            }
            return result;
        }
    }
}

using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceReptileDotNetCore.Reptile;

namespace HeritageWebserviceDotNetCore.Reptile
{
    class WebImageSaver
    {
        private static readonly Lazy<WebImageSaver> lazy = new Lazy<WebImageSaver>(() => new WebImageSaver());
        public static WebImageSaver Instance { get { return lazy.Value; } }

        public BufferBlock<string> ImageTargetBlock { get; }
        public WebImageSaver()
        {
            ImageTargetBlock = new BufferBlock<string>();
        }

        public async Task<int> SaveFileAsync(ISourceBlock<string> source)
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
                if (File.Exists(savePath))
                {
                    continue;
                }
                if (!imageUrl.StartsWith(GetIhChina.MainPage))
                {
                    imageUrl = GetIhChina.MainPage + imageUrl;
                }
                try
                {
                    Console.WriteLine("Starting to save image {0}", imageUrl);
                    wc.DownloadFile(imageUrl, savePath);
                    Console.WriteLine("Save image {0} completely", imageUrl);
                }
                catch(WebException e)
                {
                    Console.WriteLine("Save image {0} error",imageUrl);
                    Console.WriteLine(e);
                }catch(Exception e)
                {
                    Console.WriteLine("unhandled error");
                    Console.WriteLine(e);
                }
            }
            return 1;
        }
    }
}
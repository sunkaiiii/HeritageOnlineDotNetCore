using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
namespace HeritageWebserviceDotNetCore.Reptile
{
    class WebImageSaver
    {
        private static readonly Lazy<WebImageSaver> lazy = new Lazy<WebImageSaver>(()=>new WebImageSaver());
        public static WebImageSaver Instance {get {return lazy.Value;}}

        public BufferBlock<string> ImageTargetBlock{get;}
        public WebImageSaver()
        {
            ImageTargetBlock=new BufferBlock<string>();
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
                if(File.Exists(savePath))
                {
                    continue;
                }
                wc.DownloadFile(imageUrl, savePath);
            }
            return 1;
        }
    }
}
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using HeritageWebserviceReptileDotNetCore.Reptile;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace HeritageWebserviceDotNetCore.Reptile
{
    public class WebImageSaver
    {
        private static readonly Lazy<WebImageSaver> lazy = new Lazy<WebImageSaver>(() => new WebImageSaver());
        public static WebImageSaver Instance { get { return lazy.Value; } }

        public BufferBlock<string> InitImageBlock() => new BufferBlock<string>();

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
                if (!imageUrl.StartsWith(GetIhChina.MainPage))
                {
                    imageUrl = GetIhChina.MainPage + imageUrl;
                }
                var savePath = Path.Combine(Directory.GetCurrentDirectory(), "img", WebpageHelper.GetSubUrl(imageUrl));
                if (File.Exists(savePath))
                {
                    continue;
                }
                try
                {
                    Console.WriteLine("Starting to save image {0}", imageUrl);
                    wc.DownloadFile(imageUrl, savePath);
                    //CompressImageWithGuetzli(savePath); 压缩貌似有点问题，待修复
                    CorpImage(savePath);
                    Console.WriteLine("Save image {0} completely", imageUrl);
                }
                catch (WebException e)
                {
                    Console.WriteLine("Save image {0} error", imageUrl);
                    Console.WriteLine(e);
                }
                catch (Exception e)
                {
                    Console.WriteLine("unhandled error");
                    Console.WriteLine(e);
                }
            }
            return 1;
        }

        //给原始图片做一次压缩
        //并且生成一个小图
        public void CorpImage(string savePath)
        {
            const int bound = 800;
            using (Image image = Image.Load(savePath))
            {
                Console.WriteLine(image.GetType().Name);
                int resizeWidth = image.Width;
                int resizeHeight = image.Height;
                if ( image.Height>=image.Width&&resizeHeight > bound)
                {
                    double scale = (double)bound / image.Height;
                    resizeHeight = (int)(resizeHeight * scale);
                    resizeWidth = (int)(resizeWidth * scale);
                }
                else if (image.Width>image.Height &&  resizeWidth > bound)
                {
                    double scale = (double)bound / image.Width;
                    resizeWidth = (int)(resizeWidth * scale);
                    resizeHeight = (int)(resizeHeight * scale);
                }
                Console.WriteLine("Height:" + resizeHeight + " Width:" + resizeWidth);
                image.Mutate(x => x.Resize(resizeWidth, resizeHeight));
                var saveCompressName = GetComressImageName(savePath);
                image.Save(saveCompressName);
                //CompressImageWithGuetzli(saveCompressName); 压缩貌似有点问题，待修复
            }
        }

        public string GetComressImageName(string originalSavePath)
        {
            return Path.Combine(Path.GetDirectoryName(originalSavePath), Path.GetFileNameWithoutExtension(originalSavePath)) + "_compress" + Path.GetExtension(originalSavePath);
        }

        public void CompressImageWithGuetzli(string imageName)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = "guetzli";
                string argument = String.Format("--quality 84 {0} {1}", imageName, imageName);
                process.StartInfo.Arguments = argument;
                Console.WriteLine(argument);
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }
    }
}
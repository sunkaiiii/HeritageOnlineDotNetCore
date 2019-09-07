using HeritageWebserviceDotNetCore.Reptile;
using HeritageWebserviceReptileDotNetCore.Reptile;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;

namespace HeritageWebserviceDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            //  GetIhChina.StartReptile() ;
            //GetForumsWorker.GetForumsList();
            //GetSpecialTopicWorker.GetSpecialTopic();
            // GetNewsDetail.GenerateNewsDetail(@"http://www.ihchina.cn/news_details/18992.html");
            GetHeritageProjects.GetHeritageProject();

        }
    }
}

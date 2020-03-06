using HeritageWebserviceReptileDotNetCore.Reptile;

namespace HeritageWebserviceReptileDotNetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            GetIhChina.StartReptile().Wait() ;
            //GetForumsWorker.GetForumsList();
            //GetSpecialTopicWorker.GetSpecialTopic();
            // GetNewsDetail.GenerateNewsDetail(@"http://www.ihchina.cn/news_details/18992.html");
            //GetHeritageProjects.GetHeritageProject();
            //GetHeritageProjectDetailWorker.GenerateCCRDetail("http://www.ihchina.cn/ccr_detail/719");
        }
    }
}

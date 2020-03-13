using HeritageWebserviceDotNetCore.Mongodb;
using HeritageWebserviceDotNetCore.Reptile;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace HeritageWebserviceReptileDotNetCore.Reptile
{
    public static class GetPeoplePage
    {
        private static readonly string MAIN_PAGE = "http://www.ihchina.cn";

        public static void StartGetPeoplePage()
        {
            GetPeopleMainPage();
            //GetPeoplePageList();
        }

        private static void GetPeopleMainPage()
        {
            var doc = WebpageHelper.GetHttpRequestDocument(MAIN_PAGE);
            var bson = new BsonDocument();
            //获取图片和表格内容
            var tableImageNodes = doc.DocumentNode.SelectNodes("//div[@class='tab-cont']/div");
            var tableContentNodes = doc.DocumentNode.SelectNodes("//div[@class='tab-track justify']/div");
            if (tableContentNodes == null || tableContentNodes == null || tableImageNodes.Count != tableContentNodes.Count)
                return;
            var bsonArray = new BsonArray();
            for (int i = 0; i < tableImageNodes.Count; i++)
            {
                var tableBson = new BsonDocument();
                var imageNode = tableImageNodes[i];
                var contentNode = tableContentNodes[i];
                var linkNode = contentNode.SelectSingleNode(".//a");
                if (linkNode == null)
                    continue;
                var imageUrlWithAllText = imageNode.Attributes["style"].Value;
                var imageUrl = imageUrlWithAllText.Substring(imageUrlWithAllText.IndexOf("/"), imageUrlWithAllText.IndexOf(")") - imageUrlWithAllText.IndexOf("/"));
                var imageDesc = contentNode.InnerText;
                var link = linkNode.Attributes["href"].Value;
                WebImageSaver.Instance.ImageTargetBlock.Post(imageUrl);
                tableBson.Add("img", WebpageHelper.GetSubUrl(imageUrl));
                tableBson.Add("desc", imageDesc);
                tableBson.Add("link", link);
                bsonArray.Add(tableBson);
            }
            bson.Add("table", bsonArray);
            var success = WebpageHelper.TryToInsertOrUpdateABson(bson, MongodbMain.PeopleMainPage);
            if (success)
                Console.WriteLine("People page Insert or update success");
            else
                Console.WriteLine("Duplicated information in people page");
        }
    }
}

using HeritageWebServiceDotNetCore.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Service
{
    public class ForumsService
    {
        private readonly IMongoCollection<NewsList> _forumsList;
        private readonly IMongoCollection<NewsDetail> _forumsDetail;
        public ForumsService(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _forumsList = database.GetCollection<NewsList>(settings.Collections.ForumsList);
            _forumsDetail = database.GetCollection<NewsDetail>(settings.Collections.ForumsDetail);
        }

        public List<NewsList> GetForumsList(int pages)
        {
            int startNum = (pages - 1) * 20;
            if(startNum<0||startNum>_forumsList.CountDocuments(new BsonDocument()))
            {
                return new List<NewsList>();
            }
            return _forumsList.Find(n => true).SortByDescending(bson=>bson.Date).Skip(20 * (pages - 1)).Limit(20).ToList();
        }

        public NewsDetail GetForumsDetail(string link)
        {
            //这里进行了一个容错，因为之前link的数据插入的不太对，所以使用的contains
            return _forumsDetail.Find(forumsDetail => forumsDetail.Link.Contains(link)).SingleOrDefault(); 
        }
    }
}

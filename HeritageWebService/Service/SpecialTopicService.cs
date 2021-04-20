using HeritageWebServiceDotNetCore.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Service
{
    public class SpecialTopicService
    {
        private readonly IMongoCollection<NewsList> _specialList;
        private readonly IMongoCollection<NewsDetail> _specialDetail;

        public SpecialTopicService(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _specialList = database.GetCollection<NewsList>(settings.Collections.SpecialTopic);
            _specialDetail = database.GetCollection<NewsDetail>(settings.Collections.SpecialTopicDetail);
        }

        public List<NewsList> GetSpecialTopicList(int pages)
        {
            int startNum = (pages - 1) * 20;
            if(startNum<0||startNum>_specialList.CountDocuments(new BsonDocument()))
            {
                return null;
            }
            return _specialList.Find(n => true).SortByDescending(bson=>bson.Date).Skip(20 * (pages - 1)).Limit(20).ToList();
        }

        public NewsDetail GetSpecialTopicDetail(string link)
        {
            return _specialDetail.Find(detail => detail.Link.Equals(link)).SingleOrDefault();
        }
    }
}

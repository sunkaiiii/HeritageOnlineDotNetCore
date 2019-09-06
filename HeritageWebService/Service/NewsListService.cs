using HeritageWebServiceDotNetCore.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Service
{
    public class NewsListService
    {
        private readonly IMongoCollection<NewsList> _newsList;

        public NewsListService(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _newsList = database.GetCollection<NewsList>(settings.Collections.NewsListCollectionName);
        }

        public List<NewsList> Get() => _newsList.Find(newsList => true).Limit(20).ToList();

        public List<NewsList> Get(int pages)
        {
            int startNum = (pages - 1) * 20;
            if (startNum < 0 || startNum > _newsList.Count(new BsonDocument()))
            {
                return null;
            }
            return _newsList.Find(NewsList => true).Skip(20 * (pages - 1)).Limit(20).ToList();
        }

    }
}

using HeritageWebService.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebService.Service
{
    public class HeritageService
    {
        private readonly IMongoCollection<MainNewsList> _mainLists;

        //通过构造函数传入的DI检索IHeritageMainPageListSettings的实例，用于访问配置在appsetting.json的值
        public HeritageService(IHeritageMainPageListSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _mainLists = database.GetCollection<MainNewsList>(settings.MainPageCollectionName);
        }

        public List<MainNewsList> Get() => _mainLists.Find(newsList => true).ToList();
        public MainNewsList Get(string id) => _mainLists.Find<MainNewsList>(newsList => newsList.Id == id).FirstOrDefault();
        public MainNewsList Create(MainNewsList newsList)
        {
            _mainLists.InsertOne(newsList);
            return newsList;
        }
        public void Update(string id, MainNewsList newsIn) => _mainLists.ReplaceOne(news => news.Id == id, newsIn);
        public void Remove(MainNewsList newsIn) => _mainLists.DeleteOne(news => news.Id == newsIn.Id);
        public void Remove(string id) => _mainLists.DeleteOne(news => news.Id == id);
    }
}

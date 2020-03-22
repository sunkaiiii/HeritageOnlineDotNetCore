using System.Collections.Generic;
using HeritageWebService.Model;
using HeritageWebServiceDotNetCore.Model;
using MongoDB.Driver;

namespace HeritageWebServiceDotNetCore.Service
{
    public class BannerService
    {
        private readonly IMongoCollection<Banner> _banner;
        private readonly Collections _collections;

        //通过构造函数传入的DI检索IHeritageMainPageListSettings的实例，用于访问配置在appsetting.json的值
        public BannerService(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _banner = database.GetCollection<Banner>(settings.Collections.BannerCollectionName);
            _collections = settings.Collections;
        }

        public List<Banner> Get() => _banner.Find(newsList => true).Limit(7).ToList();
        public Banner Get(string id) => _banner.Find(newsList => newsList.Id == id).FirstOrDefault();
    }
}

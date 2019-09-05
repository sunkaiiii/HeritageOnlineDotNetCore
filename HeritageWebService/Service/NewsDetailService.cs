using HeritageWebServiceDotNetCore.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Service
{
    public class NewsDetailService
    {
        private readonly IMongoCollection<NewsDetail> _newsDetails;

        public NewsDetailService(IHeritageNewsDetailSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _newsDetails = database.GetCollection<NewsDetail>(settings.NewsDetailCollectionName);
        }

        public NewsDetail Get(string link) => _newsDetails.Find(newsList => newsList.link == link).FirstOrDefault();
    }
}

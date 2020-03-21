using HeritageWebServiceDotNetCore.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Service
{
    public class PeoplePageService
    {
        private readonly IMongoCollection<PeoplePage.PeopleMainPage> _peopleMainPage;
        private readonly IMongoCollection<NewsList> _peopleList;
        private readonly IMongoCollection<NewsDetail> _peopleDetail;
        public PeoplePageService(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _peopleMainPage = database.GetCollection<PeoplePage.PeopleMainPage>(settings.Collections.PeopleMainPage);
            _peopleList = database.GetCollection<NewsList>(settings.Collections.PeopleList);
            _peopleDetail = database.GetCollection<NewsDetail>(settings.Collections.PeopleDetail);
        }

        public PeoplePage.PeopleMainPage GetPeopleMainPage() => _peopleMainPage.Find(b => true).FirstOrDefault();

        public List<NewsList> GetPeopleList(int page)
        {
            int startNum = (page - 1) * 20;
            if (startNum < 0 || startNum > _peopleList.CountDocuments(new BsonDocument()))
                return null;
            return _peopleList.Find(item => true).Skip(startNum).Limit(20).ToList();
        }

        public NewsDetail GetPeopleDetail(string link) => _peopleDetail.Find(item => item.Link.Equals(link)).SingleOrDefault();
    }
}

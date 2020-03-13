using HeritageWebServiceDotNetCore.Model;
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
        public PeoplePageService(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _peopleMainPage = database.GetCollection<PeoplePage.PeopleMainPage>(settings.Collections.PeopleMainPage);
        }

        public PeoplePage.PeopleMainPage GetPeopleMainPage() => _peopleMainPage.Find(b => true).FirstOrDefault();
    }
}

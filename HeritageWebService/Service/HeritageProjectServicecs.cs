using HeritageWebServiceDotNetCore.Model;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Service
{
    public class HeritageProjectServicecs
    {
        private readonly IMongoCollection<HeritageProjectMainPage> _heritageProjectMainPage;
        private readonly IMongoCollection<HeritageProject> _heritageProject;
        private readonly IMongoCollection<HeritageProjectDetail> _heritageProjectDetail;
        private readonly IMongoCollection<HeritageInheritatePeople> _heritageInheritatePeople;

        public HeritageProjectServicecs(IHeritageMongodbSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _heritageProjectMainPage = database.GetCollection<HeritageProjectMainPage>(settings.Collections.HeritageProjectMainPageName);
            _heritageProject = database.GetCollection<HeritageProject>(settings.Collections.HeritageProjectName);
            _heritageProjectDetail = database.GetCollection<HeritageProjectDetail>(settings.Collections.HeritageProjectDetilName);
            _heritageInheritatePeople = database.GetCollection<HeritageInheritatePeople>(settings.Collections.HeritageProjectInheritatePeople);
        }

        public HeritageProjectMainPage GetMainPage() => _heritageProjectMainPage.Find(n => true).FirstOrDefault();
        public List<HeritageProject> GetProjectList(int pages)
        {
            int startNum = (pages - 1) * 20;
            if(startNum<0||startNum>_heritageProject.CountDocuments(new BsonDocument()))
            {
                return null;
            }
            return _heritageProject.Find(n => true).Skip(20 * (pages - 1)).Limit(20).ToList();
        }

        public List<HeritageProject> GetFilterSearchProjectList(HeritageProject filter,int pages)
        {
            var filterBson = new BsonDocument();
            //输入的筛选条件可能是不全的
            //如果简单的使用扩展方法ToBson，或出现筛选字段为null，会没有结果
            //过滤掉所有为null的字段，将其转换为BsonDocument
            foreach (var filedName in typeof(HeritageProject).GetFields())
            {
                var filedValue = filedName.GetValue(filter);
                if(filedValue != null) 
                {
                    filterBson.Add(filedName.Name, filedValue.ToString());
                }
            }
            return GetFilterSearchProjectList(filterBson, pages);
        }

        private List<HeritageProject> GetFilterSearchProjectList(BsonDocument filter, int pages)
        {
            int startNum = (pages - 1) * 20;
            if (startNum < 0 || startNum > _heritageProject.CountDocuments(new BsonDocument()))
            {
                return null;
            }
            return _heritageProject.Find(filter).Skip(20 * (pages - 1)).Limit(20).ToList();
        }

        public HeritageProjectDetail GetProjectDetail(string link) => _heritageProjectDetail.Find(detail => detail.link == link).FirstOrDefault();

        private class SearchFilter
        {
            public string num;
            public string type;
            public string rx_time;
            public string cate;
            public string province;
            public string unit;
        }
        public Dictionary<string,HashSet<string>> GetAllCategories()
        {
            var result = _heritageProject.Find(n => true).ToList();
            var dic = new Dictionary<string, HashSet<string>>();
            var returnTypes = typeof(SearchFilter).GetFields();
            foreach(var type in returnTypes)
            {
                dic.Add(type.Name, new HashSet<string>());
            }
            foreach(var project in result)
            {
                foreach(var type in returnTypes)
                {
                    var value = project.GetType().GetField(type.Name).GetValue(project);
                    if(value !=null)
                    {
                            dic[type.Name].Add(value.ToString());
                    }
                    
                }
            }
            return dic;
        }

        public HeritageInheritatePeople GetInheritatePeople(string link)
        {
            return _heritageInheritatePeople.Find(people => people.link.Equals(link)).SingleOrDefault();
        }
    }
}

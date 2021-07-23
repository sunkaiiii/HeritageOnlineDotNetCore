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
            if (startNum < 0 || startNum > _heritageProject.CountDocuments(new BsonDocument()))
            {
                return null;
            }
            return _heritageProject.Find(n => true).SortByDescending(bson=>bson.Rx_time).Skip(20 * (pages - 1)).Limit(20).ToList();
        }

        public List<HeritageProject> GetFilterSearchProjectList(HeritageProject filter, int pages)
        {
            var filterBson = new BsonDocument();
            //输入的筛选条件可能是不全的
            //如果简单的使用扩展方法ToBson，或出现筛选字段为null，会没有结果
            //过滤掉所有为null的字段，将其转换为BsonDocument
            foreach (var filedName in typeof(HeritageProject).GetProperties())
            {
                var filedValue = filedName.GetValue(filter);
                if (filedValue != null && filedValue.ToString().Length > 0)
                {
                    filterBson.Add(filedName.Name.ToLower(), new BsonDocument { { "$regex", filedValue.ToString() }, { "$options", "i" } });
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
            var result = _heritageProject.Find(filter).Skip(20 * (pages - 1)).Limit(20).ToList();
            return result;
        }

        public HeritageProjectDetail GetProjectDetail(string link) => _heritageProjectDetail.Find(detail => detail.Link == link).FirstOrDefault();

        private class SearchFilter
        {
            public string Num { get; set; }
            public string Type { get; set; }
            public string Rx_time { set; get; }
            public string Cate { get; set; }
            public string Province { get; set; }
            public string Unit { get; set; }
        }
        public Dictionary<string, string> GetAllCategories()
        {
            var dic = new Dictionary<string, string>();
            var returnTypes = typeof(SearchFilter).GetProperties();

            foreach(var type in returnTypes)
            {
                dic.Add(type.Name.ToLower(), new HashSet<string>());
            }
            foreach(var project in result)
            {
                var propertyName = type.Name.ToLower();
                if (searchKeyToSearchName.ContainsKey(propertyName))
                {
                    dic.Add(propertyName, searchKeyToSearchName[propertyName]);
                }
            }
            return dic;
        }

        private static readonly Dictionary<string, string> searchKeyToSearchName = new Dictionary<string, string>()
        {
            { "num","编号" },
            { "type","类别" },
            { "rx_time","入选批次" },
            { "cate","项目类型" },
            { "province","申请省份" },
            { "unit","项目所属地区" },
        };

        public HeritageInheritatePeople GetInheritatePeople(string link)
        {
            return _heritageInheritatePeople.Find(people => people.Link.Equals(link)).SingleOrDefault();
        }
    }
}

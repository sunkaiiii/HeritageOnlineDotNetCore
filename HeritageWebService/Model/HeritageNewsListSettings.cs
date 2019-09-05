using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageNewsListSettings : IHeritageNewsListSettings
    {
        public string NewsListCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
    public interface IHeritageNewsListSettings
    {
        string NewsListCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

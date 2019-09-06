using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageMongodbSettings : IHeritageMongodbSettings
    {
        public string ConnectionString { get; set; }
        public Collections Collections { get; set; }
        public string DatabaseName { get; set; }
    }

    public class Collections
    {
        public string MainPageCollectionName { get; set; }
        public string NewsDetailCollectionName { get; set; }
        public string NewsListCollectionName { get; set; }
    }
    public interface IHeritageMongodbSettings
    {
        string ConnectionString { get; set; }
        Collections Collections { get; set; }
        string DatabaseName { get; set; }
    }
}

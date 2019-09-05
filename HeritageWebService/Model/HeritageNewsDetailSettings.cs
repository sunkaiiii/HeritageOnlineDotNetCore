using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class HeritageNewsDetailSettings : IHeritageNewsDetailSettings
    {
        public string NewsDetailCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IHeritageNewsDetailSettings
    {
        string NewsDetailCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}

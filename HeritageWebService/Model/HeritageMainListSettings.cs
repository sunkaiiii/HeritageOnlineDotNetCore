using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebService.Model
{
    public class HeritageMainPageListSettings : IHeritageMainPageListSettings
    {
        public string MainPageCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IHeritageMainPageListSettings
        {
            string MainPageCollectionName { get; set; }
            string ConnectionString { get; set; }
            string DatabaseName { get; set; }
        }
    }

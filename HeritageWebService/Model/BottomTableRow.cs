using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeritageWebServiceDotNetCore.Model
{
    public class BottomTableRow
    {
        public string link;
        public List<TableContent> content;
        public class TableContent
        {
            public string key;
            public string value;
        }
    }
}

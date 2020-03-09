using System;
using System.Collections.Generic;
using System.Text;

namespace HeritageWebserviceReptileDotNetCore.DebugHelper
{
    public static class DebugHelperTools
    {
        public static bool IsDebugMode()
        {
#if DEBUG
            return true;
#endif
            return false;
        }
    }
}

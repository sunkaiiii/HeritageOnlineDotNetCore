using System;
namespace HeritageWebServiceDotNetCore.Model
{
    public class BaseRequest
    {
        public string AndroidVersion { get; set; }
        public string Language { get; set; }
        public string ModelName { get; set; }
        public string BrandName { get; set; }
        public string Version { get; set; }
        public string From { get; set; }

        public override string ToString()
        {
            return "AndroidVersion: " + AndroidVersion + "\n"
                + "Language: " + Language + "\n"
                + "ModelName: " + ModelName + "\n"
                + "BrandName: " + BrandName + "\n"
                + "Version: " + Version + "\n"
                + "From: " + From + "\n";
        }
    }
}
